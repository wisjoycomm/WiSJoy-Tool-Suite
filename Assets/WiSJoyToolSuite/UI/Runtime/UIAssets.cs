
using UnityEngine;

namespace WiSJoy.UI
{
    [CreateAssetMenu(fileName = "UIAssets", menuName = "WiSJoy/UI/UIAssets")]
    public class UIAssets : ScriptableObject
    {
        public UIAssetsEntry[] UIEntries;

#if UNITY_EDITOR
        public void RenameByUIID()
        {
            string uipath = "Assets/WiSJoyToolSuite/Manager/UI/Sources";
            if (!UnityEditor.AssetDatabase.IsValidFolder(uipath))
            {
                UnityEditor.AssetDatabase.CreateFolder("Assets/WiSJoyToolSuite/Manager/UI/Sources", "UIs");
            }
            foreach (var uientry in UIEntries)
            {
                string path = UnityEditor.AssetDatabase.GetAssetPath(uientry.UI);
                string extension = System.IO.Path.GetExtension(path);
                string filename = System.IO.Path.GetFileNameWithoutExtension(path);
                string newpath = $"{uipath}/{filename}.{extension}";
                UnityEditor.AssetDatabase.MoveAsset(path, newpath);
                string newname = $"{uientry.Key}";
                UnityEditor.AssetDatabase.RenameAsset(newpath, newname);
            }
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.EditorUtility.DisplayDialog("Rename By UI ID", "Rename By UI ID and Move to Path Complete", "OK");
        }
#endif
    }

    public class UIAssetsEntry
    {
        public string Key;
        public bool Additive;
        public BaseUI UI;
    }
}