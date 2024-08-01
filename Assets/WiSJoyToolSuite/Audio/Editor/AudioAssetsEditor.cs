using System.IO;
using UnityEditor;
using UnityEngine;
using WiSJoy.Extend.Editor;
namespace WiSJoy.Audio.Editor
{
    [CustomEditor(typeof(AudioAssets))]
    public class AudioAssetsEditor : UnityEditor.Editor
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
            if (GUILayout.Button("Update Script"))
            {
                GenerateID();
            }
        }

        private void GenerateID()
        {
            AudioAssets audioAssets = (AudioAssets)target;

            string musicclass = "\n\tpublic enum eMusicID\n\t{\n";
            musicclass += $"\t\tNone = 0,\n";
            for (int i = 0; i < audioAssets.MusicClips.Length; i++)
            {
                musicclass += $"\t\t{audioAssets.MusicClips[i].Key} = {i + 1},\n";
            }
            musicclass += "\t}\n";

            string sfxclass = "\tpublic enum eSFXID\n\t{\n";
            sfxclass += $"\t\tNone = 0,\n";
            for (int i = 0; i < audioAssets.SFXClips.Length; i++)
            {
                sfxclass += $"\t\t{audioAssets.SFXClips[i].Key} = {i + 1},\n";
            }
            sfxclass += "\t}\n";

            string musicid = "\t\tpublic static string[] MUSICID = new string[]\n\t{\n";
            musicid += $"\t\t\"None\",\n";
            for (int i = 0; i < audioAssets.MusicClips.Length; i++)
            {
                musicid += $"\t\t\"{audioAssets.MusicClips[i].Key}\",\n";
            }
            musicid += "\t};\n";
            string sfxid = "\n\t\tpublic static string[] SFXID = new string[]\n\t{\n";
            sfxid += $"\t\"None\",\n";
            for (int i = 0; i < audioAssets.SFXClips.Length; i++)
            {
                sfxid += $"\t\t\"{audioAssets.SFXClips[i].Key}\",\n";
            }
            sfxid += "\t};\n";

            string audioid = $"\tpublic class AudioID\n{{\n{musicid}\n{sfxid}\n}}\n";

            string allGenerators = musicclass + sfxclass + audioid;
            string filePath = UtilityEditor.GetFilePathInAsset("AudioID", ".cs");
            string existingCode = File.ReadAllText(filePath);
            // Replace existing methods with new ones
            if (!string.IsNullOrEmpty(allGenerators))
            {
                // Example: Replace existing methods
                int startIndex = existingCode.IndexOf("#region Auto Generators") + "#region Auto Generators".Length;
                int endIndex = existingCode.IndexOf("#endregion", startIndex);
                existingCode = existingCode.Remove(startIndex, endIndex - startIndex);
                existingCode = existingCode.Insert(startIndex, allGenerators);
            }
            string audioIdPath = Application.dataPath + "/WiSJoyToolSuite/Manager/Audio/Runtime/AudioID.cs";

            File.WriteAllText(audioIdPath, existingCode);
            EditorUtility.DisplayDialog("Generate ID", "Generate ID Complete", "OK");
        }
    }
}