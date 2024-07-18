using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using WiSdom.SaveSystem;
using UnityEditor;
using Cysharp.Threading.Tasks;
public class DataEditorWindow : EditorWindow
{
    private string message = "Data Editor";
    private SerializedProperty _data;
    private SerializedObject _so;
    private SerializedProperty _sp;
    private bool _saveinbyte = false;
    private Vector2 scrollPosition;
    [MenuItem("WiSJoy/Data Editor")]
    public static void ShowWindow()
    {
        var window = GetWindow<DataEditorWindow>("Data Editor");
        window.minSize = new Vector2(500, 700);
        window.autoRepaintOnSceneChange = true;
    }
    private void OnEnable()
    {
        _so = new SerializedObject(DataManager.I);
        _data = _so.FindProperty("Data");
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label(message, EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.Height(50));
        GUILayout.Space(10);
        if (!DataManager.IsInstanceValid())
        {
            message = "DataManager not found";
            // If DataManager is not found, show a button to create one
            if (GUILayout.Button("Create DataManager"))
            {
                _so = new SerializedObject(DataManager.I);
                _data = _so.FindProperty("Data");
                message = "Data Editor";
            }
            return;
        }
        else
        {
            if (_so == null)
            {
                _so = new SerializedObject(DataManager.I);
                _data = _so.FindProperty("Data");
            }
        }
        if (EditorGUILayout.Toggle("Save in byte", _saveinbyte) != _saveinbyte)
        {
            _saveinbyte = !_saveinbyte;
            // Set Define Symbols for current platform
            Debug.Log(PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup));
            if (!_saveinbyte)
            {
                if (!PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Contains("USE_JSON"))
                {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup) + ";USE_JSON");
                }
            }
            else
            {
                // Remove Define Symbols for current platform
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Replace(";USE_JSON", ""));
            }
            // Refresh
            AssetDatabase.Refresh();

        }
        // message
        GUILayout.Label($"true : save in byte[]\nfalse : save in json", EditorStyles.helpBox, GUILayout.ExpandWidth(true));
        GUILayout.Space(10);

        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        // Button to save all data
        if (GUILayout.Button("Save All", GUILayout.Width(150)))
        {
            DataManager.I.SaveAllData().Forget();
            Debug.Log("Data Saved");
        }
        // Button to load all data
        if (GUILayout.Button("Load All", GUILayout.Width(150)))
        {
            DataManager.I.LoadAllData().Forget();
            Debug.Log("Data Loaded");
        }
        // Button to clear all data
        if (GUILayout.Button("Clear All", GUILayout.Width(150)))
        {
            DataManager.I.ClearAllData();
            Debug.Log("Data Cleared");
        }

        GUILayout.EndHorizontal();
        GUILayout.Label($"Data Folder : Save path\nOpen Class : DataClass.cs \nManager : DataManager.cs", EditorStyles.helpBox, GUILayout.ExpandWidth(true));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Data Folder", GUILayout.Width(150)))
        {
            string path = Application.persistentDataPath + "/";
            EditorUtility.RevealInFinder(path);
        }
        if (GUILayout.Button("DataClass.cs", GUILayout.Width(150)))
        {
            string path = UtilityEditor.GetFilePathInAsset("DataClass");
            AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<MonoScript>(path));
        }
        if (GUILayout.Button("DataManager.cs", GUILayout.Width(150)))
        {
            string path = UtilityEditor.GetFilePathInAsset("DataManager");
            AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<MonoScript>(path));
        }
        GUILayout.EndHorizontal();
        GUILayout.Label("Generate Script using Roslyn", EditorStyles.helpBox, GUILayout.ExpandWidth(true));
        if (GUILayout.Button("Update Script"))
        {
            GenerateDataManager();
            // Reload
            AssetDatabase.Refresh();

        }

        GUILayout.EndVertical();
        // Display all data
        if (GUILayout.Button("Go to Data Manager"))
        {
            Selection.activeObject = DataManager.I;
        }
        GUILayout.Space(10);
        // Show data from GameData, can edit if needed using the SerializedObject
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        _so.Update();
        _sp = _so.GetIterator();
        _sp.NextVisible(true);
        do
        {
            EditorGUILayout.PropertyField(_sp, true);
        } while (_sp.NextVisible(false));
        _so.ApplyModifiedProperties();
        EditorGUILayout.EndScrollView();

        #region Show All Data
        #endregion

    }


    #region Generate DataManager

    private void GenerateDataManager()
    {
        // Get all classes from DataClass.cs
        string dataclassPath = UtilityEditor.GetFilePathInAsset("DataClass", ".cs");
        string sourceCode = File.ReadAllText(dataclassPath);
        SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);
        var root = (CompilationUnitSyntax)tree.GetRoot();
        var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

        List<ClassInfo> classes = new List<ClassInfo>();

        foreach (var classDecl in classDeclarations)
        {
            var properties = classDecl.Members.OfType<PropertyDeclarationSyntax>()
                .Select(p => new PropertyInfo { Name = p.Identifier.ValueText, Type = p.Type.ToString() })
                .ToList();

            classes.Add(new ClassInfo { Name = classDecl.Identifier.ValueText, Properties = properties });
        }

        GenerateGameDataClass(classes);
        GenerateDataManagerClass(classes);
    }

    private void GenerateGameDataClass(List<ClassInfo> classes)
    {
        string filePath = UtilityEditor.GetFilePathInAsset("GameData", ".cs");

        string existingCode = File.ReadAllText(filePath);

        // create constructor for GameData.cs
        string constructor = "\t\tpublic GameData()\n\t\t{\n\t\t\t";
        foreach (var classInfo in classes)
        {
            constructor += $"{classInfo.Name} = new {classInfo.Name}();\n\t\t\t";
        }
        constructor += "\n\t\t}";
        constructor += "\n\t\t";
        if (!string.IsNullOrEmpty(constructor))
        {
            int insertionIndex = existingCode.IndexOf("#region Constructor") + "#region Field Generators".Length;
            int endIndex = existingCode.IndexOf("#endregion", insertionIndex);
            existingCode = existingCode.Remove(insertionIndex, endIndex - insertionIndex);
            existingCode = existingCode.Insert(insertionIndex, constructor);

            Debug.Log("GameData updated with new fields.");
        }
        else
        {
            Debug.Log("No new fields were added to GameData.");
        }


        // Create field generators from GameData.cs
        string fieldGenerators = "";
        foreach (var classInfo in classes)
        {
            string fieldDeclaration = $"\n\t\tpublic {classInfo.Name} {classInfo.Name} = new {classInfo.Name}();";
            fieldGenerators += fieldDeclaration;
        }
        fieldGenerators += "\n\t\t";

        // replace new fields at the end or a specific location if needed

        if (!string.IsNullOrEmpty(fieldGenerators))
        {
            int insertionIndex = existingCode.IndexOf("#region Field Generators") + "#region Field Generators".Length;
            int endIndex = existingCode.IndexOf("#endregion", insertionIndex);
            existingCode = existingCode.Remove(insertionIndex, endIndex - insertionIndex);
            existingCode = existingCode.Insert(insertionIndex, fieldGenerators);

            Debug.Log("GameData updated with new fields.");
        }
        else
        {
            Debug.Log("No new fields were added to GameData.");
        }

        File.WriteAllText(filePath, existingCode);

    }

    private void GenerateDataManagerClass(List<ClassInfo> classes)
    {
        string filePath = UtilityEditor.GetFilePathInAsset("DataManager", ".cs");

        string existingCode = File.ReadAllText(filePath);

        // Generate for LoadAllData, SaveAllData, ClearAllData
        string allGenerators = "";
        foreach (var classInfo in classes)
        {
            string loadMethod = $"\n\t\tpublic async UniTask Load{classInfo.Name}Data()\n\t\t{{\n\t\t\tData.{classInfo.Name} = await SaveManager.LoadData<{classInfo.Name}>(\"{classInfo.Name}\");\n\t\t\tif(Data.{classInfo.Name} is null)\n\t\t\t{{\n\t\t\t\tData.{classInfo.Name} = new {classInfo.Name}();\n\t\t\t}}\n\t\t}}";
            string saveMethod = $"\n\t\tpublic async UniTask Save{classInfo.Name}Data()\n\t\t{{\n\t\t\tawait SaveManager.SaveData(Data.{classInfo.Name}, \"{classInfo.Name}\");\n\t\t}}";
            string clearMethod = $"\n\t\tpublic void Clear{classInfo.Name}Data()\n\t\t{{\n\t\t\tSaveManager.ClearData(\"{classInfo.Name}\");\n\t\t}}";
            allGenerators += loadMethod + saveMethod + clearMethod;
        }
        allGenerators += "\n\t\t";
        // Replace existing methods with new ones
        if (!string.IsNullOrEmpty(allGenerators))
        {
            // Example: Replace existing methods
            int startIndex = existingCode.IndexOf("#region Methods Generators") + "#region Methods Generators".Length;
            int endIndex = existingCode.IndexOf("#endregion", startIndex);
            existingCode = existingCode.Remove(startIndex, endIndex - startIndex);
            existingCode = existingCode.Insert(startIndex, allGenerators);
            Debug.Log("DataManager updated with new methods.");
        }
        else
        {
            Debug.Log("No new methods were added to DataManager.");
        }

        // Generate for Load Data, Save Data , Clear Data
        string LoadAllMethods = "";
        foreach (var classInfo in classes)
        {
            string loadMethod = $"\n\t\t\tawait Load{classInfo.Name}Data();";
            LoadAllMethods += loadMethod;
        }
        LoadAllMethods += "\n\t\t\t";
        string SaveAllMethods = "";
        foreach (var classInfo in classes)
        {
            string saveMethod = $"\n\t\t\tawait Save{classInfo.Name}Data();";
            SaveAllMethods += saveMethod;
        }
        SaveAllMethods += "\n\t\t\t";
        string ClearAllMethods = "";
        foreach (var classInfo in classes)
        {
            string clearMethod = $"\n\t\t\tSaveManager.ClearData(\"{classInfo.Name}\");";
            ClearAllMethods += clearMethod;
        }
        ClearAllMethods += "\n\t\t\t";
        // Replace existing load methods with new ones
        if (!string.IsNullOrEmpty(LoadAllMethods))
        {
            // Example: Replace existing methods
            int startIndex = existingCode.IndexOf("#region Load Data") + "#region Load Data".Length;
            // next #endregion after Load Data
            int endIndex = existingCode.IndexOf("#endregion", startIndex);
            existingCode = existingCode.Remove(startIndex, endIndex - startIndex);
            existingCode = existingCode.Insert(startIndex, LoadAllMethods);
            Debug.Log("DataManager updated with new methods.");
        }
        else
        {
            Debug.Log("No new methods were added to DataManager.");
        }
        // Replace existing save methods with new ones
        if (!string.IsNullOrEmpty(SaveAllMethods))
        {
            // Example: Replace existing methods
            int startIndex = existingCode.IndexOf("#region Save Data") + "#region Save Data".Length;
            // next #endregion after Save Data
            int endIndex = existingCode.IndexOf("#endregion", startIndex);
            existingCode = existingCode.Remove(startIndex, endIndex - startIndex);
            existingCode = existingCode.Insert(startIndex, SaveAllMethods);
            Debug.Log("DataManager updated with new methods.");
        }
        else
        {
            Debug.Log("No new methods were added to DataManager.");
        }

        // Replace existing clear methods with new ones
        if (!string.IsNullOrEmpty(ClearAllMethods))
        {
            // Example: Replace existing methods
            int startIndex = existingCode.IndexOf("#region Clear Data") + "#region Clear Data".Length;
            // next #endregion after Clear Data
            int endIndex = existingCode.IndexOf("#endregion", startIndex);
            existingCode = existingCode.Remove(startIndex, endIndex - startIndex);
            existingCode = existingCode.Insert(startIndex, ClearAllMethods);
            Debug.Log("DataManager updated with new methods.");
        }
        else
        {
            Debug.Log("No new methods were added to DataManager.");
        }


        File.WriteAllText(filePath, existingCode);

    }

    #endregion

}
