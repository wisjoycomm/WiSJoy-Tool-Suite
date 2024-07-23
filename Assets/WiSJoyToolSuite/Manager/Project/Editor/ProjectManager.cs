using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectManager : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    private string m_ProjectName = "_Project";

    [MenuItem("WiSJoy/ProjectManager")]
    public static void ShowExample()
    {
        ProjectManager wnd = GetWindow<ProjectManager>();
        wnd.titleContent = new GUIContent("ProjectManager");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Create Input Field
        TextField textField = new TextField("Project Name");
        textField.value = m_ProjectName;
        textField.RegisterValueChangedCallback(evt => m_ProjectName = evt.newValue);
        root.Add(textField);

        // Create button
        Button button = new Button();
        button.text = "Generate Project";
        button.clickable.clicked += GenerateProject;
        root.Add(button);

    }

    private void GenerateProject()
    {
        // Create Project
        string[] projectFolders = new string[] {
            "Plugins",
            "Resources",
            "Editor",
            "StreamingAssets",};
        string basePath = Application.dataPath;
        foreach (string folder in projectFolders)
        {
            if (!System.IO.Directory.Exists(basePath + "/" + folder))
            {
                Debug.Log(Application.dataPath + "/" + folder);
                // create folder by file io
                System.IO.Directory.CreateDirectory(basePath + "/" + folder);
            }
        }

        //Editor subfolders
        string[] editorfolders = new string[] {
            // "Resources",
        };
        foreach (string subfolder in editorfolders)
        {
            if (!System.IO.Directory.Exists(basePath + "/Editor/" + subfolder))
            {
                Debug.Log(basePath + "/Editor/" + subfolder);
                System.IO.Directory.CreateDirectory(basePath + "/Editor/" + subfolder);
            }
        }

        Debug.Log("Generating Project");
        // Script to create folders in structure using 
        // root is projectName

        // Create root folder
        string rootPath = Application.dataPath + "/" + m_ProjectName;
        if (!System.IO.Directory.Exists(rootPath))
        {
            Debug.Log(rootPath);
            // create folder by file io
            System.IO.Directory.CreateDirectory(rootPath);
        }
        // Create subfolders
        string[] subfolders = new string[] {
            "Scenes",
            "Scripts",
            "Prefabs",
            "Materials",
            "Shaders",
            "Plugins",
            "Resources",
            "Sources",
            "Editor" };
        foreach (string subfolder in subfolders)
        {
            if (!System.IO.Directory.Exists(rootPath + "/" + subfolder))
            {
                Debug.Log(rootPath + "/" + subfolder);
                System.IO.Directory.CreateDirectory(rootPath + "/" + subfolder);
            }
        }
        //Editor subfolders
        string[] editorSubfolders = new string[] {
            // "Resources",
        };
        foreach (string subfolder in editorSubfolders)
        {
            if (!System.IO.Directory.Exists(rootPath + "/Editor/" + subfolder))
            {
                Debug.Log(rootPath + "/Editor/" + subfolder);
                System.IO.Directory.CreateDirectory(rootPath + "/Editor/" + subfolder);
            }
        }

        // Scripts subfolders
        string[] scriptsSubfolders = new string[] {
            "Editor",
            "Runtime",
        };

        foreach (string subfolder in scriptsSubfolders)
        {
            if (!System.IO.Directory.Exists(rootPath + "/Scripts/" + subfolder))
            {
                Debug.Log(rootPath + "/Scripts/" + subfolder);
                System.IO.Directory.CreateDirectory(rootPath + "/Scripts/" + subfolder);
            }
        }

        // Create Sources subfolders
        string[] sourcesSubfolders = new string[] {
            "Audios",
            "Fonts",
            "Images",
            "Models",
            "Textures",
            "Videos",
            "Animations",};

        foreach (string subfolder in sourcesSubfolders)
        {
            if (!System.IO.Directory.Exists(rootPath + "/Sources/" + subfolder))
            {
                Debug.Log(rootPath + "/Sources/" + subfolder);
                System.IO.Directory.CreateDirectory(rootPath + "/Sources/" + subfolder);
            }
        }

        // Create Scenes subfolders

        //Refresh the project window
        AssetDatabase.Refresh();
    }
}
