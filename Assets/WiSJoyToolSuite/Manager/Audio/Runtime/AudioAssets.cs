using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
namespace WiSJoy.Manager.Audio
{

    [CreateAssetMenu(fileName = "AudioAssets", menuName = "WiSJoy/Audio/AudioAssets")]
    public class AudioAssets : ScriptableObject
    {
        public AudioClipInfo[] MusicClips = new AudioClipInfo[0];
        public AudioClipInfo[] SFXClips = new AudioClipInfo[0];

#if UNITY_EDITOR
        public void RenameByAudioID()
        {
            string musicpath = "Assets/WiSJoyToolSuite/Manager/Audio/Sources/Musics";
            if (!AssetDatabase.IsValidFolder(musicpath))
            {
                AssetDatabase.CreateFolder("Assets/WiSJoyToolSuite/Manager/Audio/Sources", "Musics");
            }
            foreach (var clipinfo in MusicClips)
            {
                for (int i = 0; i < clipinfo.Clips.Length; i++)
                {
                    string path = AssetDatabase.GetAssetPath(clipinfo.Clips[i]);
                    string extension = Path.GetExtension(path);
                    string filename = Path.GetFileNameWithoutExtension(path);
                    string newpath = $"{musicpath}/{filename}.{extension}";
                    AssetDatabase.MoveAsset(path, newpath);
                    string newname = $"{clipinfo.Name}_{i}";
                    AssetDatabase.RenameAsset(newpath, newname);
                }
            }
            string sfxpath = "Assets/WiSJoyToolSuite/Manager/Audio/Sources/SFXs";
            if (!AssetDatabase.IsValidFolder(sfxpath))
            {
                AssetDatabase.CreateFolder("Assets/WiSJoyToolSuite/Manager/Audio/Sources", "SFXs");
            }
            foreach (var clipinfo in SFXClips)
            {
                for (int i = 0; i < clipinfo.Clips.Length; i++)
                {
                    string path = AssetDatabase.GetAssetPath(clipinfo.Clips[i]);
                    string extension = Path.GetExtension(path);
                    string filename = Path.GetFileNameWithoutExtension(path);
                    string newpath = $"{sfxpath}/{filename}.{extension}";
                    AssetDatabase.MoveAsset(path, newpath);
                    string newname = $"{clipinfo.Name}_{i}";
                    AssetDatabase.RenameAsset(newpath, newname);
                }
            }

            EditorUtility.SetDirty(this);
            EditorUtility.DisplayDialog("Rename By Audio ID", "Rename By Audio ID and Move to Path Complete", "OK");
        }

        public void RemapExtension()
        {
            string[] allextension = new string[] { ".mp3", ".wav", ".ogg" };
            string toextension = ".ogg";

            for (int i = 0; i < MusicClips.Length; i++)
            {
                for (int j = 0; j < MusicClips[i].Clips.Length; j++)
                {
                    string path = AssetDatabase.GetAssetPath(MusicClips[i].Clips[j]);
                    string extension = Path.GetExtension(path);
                    if (string.CompareOrdinal(extension, toextension) == 0)
                    {
                        continue;
                    }
                    string newpath = path.Replace(extension, toextension);
                    if (File.Exists(newpath))
                    {
                        MusicClips[i].Clips[j] = AssetDatabase.LoadAssetAtPath<AudioClip>(newpath);
                    }
                }
            }

            for (int i = 0; i < SFXClips.Length; i++)
            {
                for (int j = 0; j < SFXClips[i].Clips.Length; j++)
                {
                    string path = AssetDatabase.GetAssetPath(SFXClips[i].Clips[j]);
                    string extension = Path.GetExtension(path);
                    if (string.CompareOrdinal(extension, toextension) == 0)
                    {
                        continue;
                    }
                    string newpath = path.Replace(extension, toextension);
                    if (File.Exists(newpath))
                    {
                        SFXClips[i].Clips[j] = AssetDatabase.LoadAssetAtPath<AudioClip>(newpath);
                    }
                }
            }

            EditorUtility.SetDirty(this);
            EditorUtility.DisplayDialog("Remap Extension", "Remap Extension Complete", "OK");
        }
    }

#endif

    [Serializable]
    public class AudioClipInfo
    {
        public string Name;
        public AudioClip[] Clips;
    }
}