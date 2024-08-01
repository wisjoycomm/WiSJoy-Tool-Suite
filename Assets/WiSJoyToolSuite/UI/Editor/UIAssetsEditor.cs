using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using WiSJoy.Extend.Editor;

namespace WiSJoy.UI.Editor
{
    [CustomEditor(typeof(UIAssets))]
    public class UIAssetsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Rename By UI ID"))
            {
                ((UIAssets)target).RenameByUIID();
            }
            if (GUILayout.Button("Update Script"))
            {
                GenerateID();
            }
        }

        private void GenerateID()
        {
            UIAssets uiAssets = (UIAssets)target;

            string uiclass = "\n\tpublic enum UIID\n\t{\n";
            uiclass += $"\t\tNone = 0,\n";
            for (int i = 0; i < uiAssets.UIEntries.Length; i++)
            {
                uiclass += $"\t\t{uiAssets.UIEntries[i].Key} = {i + 1},\n";
            }
            uiclass += "\t}\n";

            string uiid = "\t\tpublic static string[] UIID = new string[]\n\t{\n";
            uiid += $"\t\t\"None\",\n";
            for (int i = 0; i < uiAssets.UIEntries.Length; i++)
            {
                uiid += $"\t\t\"{uiAssets.UIEntries[i].Key}\",\n";
            }
            uiid += "\t};\n";

            string allGenerators = uiclass + uiid;
            string filePath = UtilityEditor.GetFilePathInAsset("UIID", ".cs");
            string existingCode = File.ReadAllText(filePath);

            // Replace existing methods with new ones
            if (!string.IsNullOrEmpty(allGenerators))
            {
                // Example: Replace existing methods
                int startIndex = existingCode.IndexOf("#region Auto Generators") + "#region Auto Generators".Length;
                int endIndex = existingCode.IndexOf("#endregion", startIndex);
                existingCode = existingCode.Remove(startIndex, endIndex - startIndex);
                existingCode = existingCode.Insert(startIndex, allGenerators);
            }
            string audioIdPath = Application.dataPath + "/WiSJoyToolSuite/Manager/UI/Runtime/UIID.cs";

            File.WriteAllText(audioIdPath, existingCode);
            EditorUtility.DisplayDialog("Generate ID", "Generate ID Complete", "OK");
        }
    }
}