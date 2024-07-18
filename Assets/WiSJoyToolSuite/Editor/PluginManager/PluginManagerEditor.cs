using UnityEngine;
using UnityEditor;

public class PluginManagerEditor : EditorWindow
{
    private PluginConfig pluginConfig;
    private Vector2 scrollPosition;
    private ePlugin newPlugin = ePlugin.None;
    private bool checkDuplicate = false;
    private string message = "Loading...";

    [MenuItem("WiSJoy/Plugin Manager", priority = 0)]
    public static void ShowWindow()
    {
        var window = GetWindow<PluginManagerEditor>("Plugin Manager");
        window.minSize = new Vector2(500, 400);
        window.maxSize = new Vector2(600, 800);
    }
    private GUIStyle headerStyle = null;

    void OnEnable()
    {
        pluginConfig = AssetDatabase.LoadAssetAtPath<PluginConfig>("Assets/WiSJoyToolSuite/Editor/PluginManager/PluginConfig.asset");
        foreach (var plugin in pluginConfig.plugins)
        {
            plugin.ChangeDefineSymbol();
        }
    }

    void OnGUI()
    {
        if (pluginConfig == null)
        {
            GUILayout.Label("Plugin Config not loaded!", EditorStyles.boldLabel);
            return;
        }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Open Plugin Config"))
        {
            Selection.activeObject = pluginConfig;
            message = "Plugin Config opened!";
        }
        if (GUILayout.Button("Open enum file"))
        {
            // open PluginConfig.cs in ide
            string path = "Assets/WiSdomToolSuite/Editor/PluginManager/PluginConfig.cs";
            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(path, 0);
            message = "PluginConfig.cs opened!";
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.Label(message, EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.Height(50));
        GUILayout.Space(10);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        EditorGUILayout.BeginVertical(); // Begin vertical grouping

        EditorGUILayout.BeginHorizontal(); // Create a header row
        GUILayout.Label("", EditorStyles.boldLabel, GUILayout.Width(50));
        GUILayout.Label("Name", EditorStyles.boldLabel, GUILayout.Width(150));
        GUILayout.Label("Define Symbol", EditorStyles.boldLabel, GUILayout.Width(200));
        GUILayout.Label("", GUILayout.Width(60)); // For remove button spacing
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < pluginConfig.plugins.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            pluginConfig.plugins[i].IsEnable = EditorGUILayout.Toggle("", pluginConfig.plugins[i].IsEnable, GUILayout.Width(50));
            newPlugin = (ePlugin)EditorGUILayout.EnumPopup("", pluginConfig.plugins[i].Name, GUILayout.Width(150));
            if (newPlugin != pluginConfig.plugins[i].Name)
            {
                checkDuplicate = false;
                // check if the new plugin is already in the list
                for (int j = 0; j < pluginConfig.plugins.Count; j++)
                {
                    if (pluginConfig.plugins[j].Name == newPlugin)
                    {
                        checkDuplicate = true;
                        break;
                    }
                }
                if (!checkDuplicate)
                {
                    pluginConfig.plugins[i].Name = newPlugin;
                    pluginConfig.plugins[i].ChangeDefineSymbol();
                    message = $"{pluginConfig.plugins[i].Name} updated!";
                }
                else
                {
                    // Show small popup
                    message = "Plugin already exists!";
                }
            }
            pluginConfig.plugins[i].DefineSymbol = EditorGUILayout.TextField("", pluginConfig.plugins[i].DefineSymbol, GUILayout.Width(200));

            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                RemovePlugin(i);
                break; // Exit loop to avoid modifying the collection during iteration
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical(); // End vertical grouping
        if (GUILayout.Button("+", GUILayout.Width(30), GUILayout.Height(30)))
        {
            AddNewPlugin();
        }


        if (GUILayout.Button("Save Changes"))
        {
            // Add define symbols, remove if not enabled or not defined
            foreach (var plugin in pluginConfig.plugins)
            {
                if (plugin.IsEnable && !string.IsNullOrEmpty(plugin.DefineSymbol))
                {
                    if (!PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Contains(plugin.DefineSymbol))
                    {
                        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup) + ";" + plugin.DefineSymbol);
                    }
                }
                else
                {
                    if (PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Contains(plugin.DefineSymbol))
                    {
                        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Replace($";{plugin.DefineSymbol}", ""));
                    }
                }
            }

            EditorUtility.SetDirty(pluginConfig);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            message = "Changes saved!";
        }

        EditorGUILayout.EndScrollView();

    }

    private void AddNewPlugin()
    {
        var list = new System.Collections.Generic.List<PluginData>(pluginConfig.plugins);
        list.Add(new PluginData { Name = ePlugin.None, IsEnable = false, DefineSymbol = "" });
        pluginConfig.plugins = list;
        EditorUtility.SetDirty(pluginConfig);
        message = "New plugin added!";
    }

    private void RemovePlugin(int index)
    {
        var list = new System.Collections.Generic.List<PluginData>(pluginConfig.plugins);
        // Remove define symbol
        if (!string.IsNullOrEmpty(pluginConfig.plugins[index].DefineSymbol))
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Replace(pluginConfig.plugins[index].DefineSymbol, ""));
        }
        list.RemoveAt(index);
        pluginConfig.plugins = list;
        EditorUtility.SetDirty(pluginConfig);

        message = $"{pluginConfig.plugins[index].Name} removed!";
    }
}
