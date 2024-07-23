using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WiSJoyTool
{
    [MenuItem("WiSJoy/Folder/Open Data")]
    public static void OpenDataFolder()
    {
        string path = Application.persistentDataPath + "/";
        EditorUtility.RevealInFinder(path);
    }
}
