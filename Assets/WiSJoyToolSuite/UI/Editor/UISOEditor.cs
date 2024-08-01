using DG.DemiEditor;
using UnityEditor;
using UnityEngine;
using WiSJoy.Extend.Editor;
using WiSJoy.UI;

namespace WisJoy.UI.Editor
{
    [CustomEditor(typeof(UISO))]
    public class UISOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Load"))
            {
                string path = UtilityEditor.GetFilePathInAsset("UIAssets", ".asset");
                if (path.IsNullOrEmpty())
                {
                    Debug.LogError("UIAssets not found");
                    return;
                }
                UIAssets uiAssets = AssetDatabase.LoadAssetAtPath<UIAssets>(path);
                ((UISO)target).Load(uiAssets);

                EditorUtility.SetDirty(target);
            }
        }

    }

}