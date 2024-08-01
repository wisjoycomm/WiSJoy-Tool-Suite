using DG.DemiEditor;
using UnityEditor;
using UnityEngine;
using WiSJoy.Extend.Editor;

namespace WiSJoy.Audio.Editor
{
    [CustomEditor(typeof(AudioSO))]
    public class AudioSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Load"))
            {
                var path = UtilityEditor.GetFilePathInAsset("AudioAssets", ".asset");
                if (path.IsNullOrEmpty())
                {
                    Debug.LogError("AudioAssets not found");
                    return;
                }
                AudioAssets audioAssets = AssetDatabase.LoadAssetAtPath<AudioAssets>(path);
                ((AudioSO)target).Load(audioAssets);

                EditorUtility.SetDirty(target);
            }
        }


    }
}