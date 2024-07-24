using DG.DemiEditor;
using UnityEditor;
using UnityEngine;
using WiSJoy.Manager.UI;

namespace WisJoy.Manager.UI.Editor
{
    [CustomEditor(typeof(UISO))]
    public class UISOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if(GUILayout.Button("Load"))
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