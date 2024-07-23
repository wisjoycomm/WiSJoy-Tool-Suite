using UnityEditor;
using UnityEngine;
using WiSJoy.Manager.Audio;
[CustomEditor(typeof(AudioAssets))]
public class AudioAssetsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Rename By Audio ID"))
        {
            ((AudioAssets)target).RenameByAudioID();
        }
        if (GUILayout.Button("Remap Extension"))
        {
            ((AudioAssets)target).RemapExtension();
        }
    }
}
