using UnityEditor;
using UnityEngine;
namespace WiSJoy.Extend.Editor
{
    public class WiSJoyTool
    {
        [MenuItem("WiSJoy/Folder/Open Data")]
        public static void OpenDataFolder()
        {
            string path = Application.persistentDataPath + "/";
            EditorUtility.RevealInFinder(path);
        }
    }
}