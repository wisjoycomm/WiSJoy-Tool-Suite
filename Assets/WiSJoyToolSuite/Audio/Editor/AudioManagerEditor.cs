using UnityEditor;
using UnityEngine;
namespace WiSJoy.Audio.Editor
{
    public class AudioManagerEditor : UnityEditor.EditorWindow
    {
        static AudioManagerEditor window;
        [MenuItem("WiSJoy/AudioManager")]
        public static void ShowWindow()
        {
            window = GetWindow<AudioManagerEditor>("AudioManager");
            window.Show();
        }
        private void OnGUI()
        {
            GUILayout.Label("AudioManager", EditorStyles.boldLabel);
            
            if (GUILayout.Button("Load"))
            {
                Debug.Log("Load");
            }
            if (GUILayout.Button("Save"))
            {
                Debug.Log("Save");
            }
        }
    }
}
