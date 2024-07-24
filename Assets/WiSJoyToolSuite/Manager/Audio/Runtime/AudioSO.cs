using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WiSJoy.Manager.Audio
{
    [CreateAssetMenu(fileName = "AudioSO", menuName = "WiSJoy/Audio/AudioSO")]
    public class AudioSO : ScriptableObject
    {
        public MusicSOEntry[] MusicClips;
        public SFXSOEntry[] SFXClips;

        public void Load(AudioAssets audioAssets)
        {
            for (int i = 0; i < MusicClips.Length; i++)
            {
                for (int j = 0; j < audioAssets.MusicClips.Length; j++)
                {
                    if (audioAssets.MusicClips[j].Key == MusicClips[i].Key.ToString())
                    {
                        MusicClips[i].Clips = audioAssets.MusicClips[j].Clips;
                        break;
                    }
                }
            }

            for (int i = 0; i < SFXClips.Length; i++)
            {
                for (int j = 0; j < audioAssets.SFXClips.Length; j++)
                {
                    if (audioAssets.SFXClips[j].Key == SFXClips[i].Key.ToString())
                    {
                        SFXClips[i].Clips = audioAssets.SFXClips[j].Clips;
                        break;
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class MusicSOEntry
    {
        public MusicID Key;
        public AudioClip[] Clips;
    }
    [System.Serializable]
    public class SFXSOEntry
    {
        public SFXID Key;
        public AudioClip[] Clips;
    }
}