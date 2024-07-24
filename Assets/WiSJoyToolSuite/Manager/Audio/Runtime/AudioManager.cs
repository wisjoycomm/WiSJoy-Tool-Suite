using UnityEngine;
using UnityEngine.Audio;
using WiSJoy.DesignPattern;
using DG.Tweening;
using System.Collections;

namespace WiSJoy.Manager.Audio
{
    public class AudioManager : MonoBehaviour
    {
        #region Fields
        public AudioSO SO;
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;

        [SerializeField] private AudioMixer _masterMixer;
        [SerializeField] private AudioMixer _musicMixerGroup;
        [SerializeField] private AudioMixer _sfxMixerGroup;
        #endregion

        #region Life Cycle
        private void Awake()
        {
            MessageBus.I.Subscribe<AudioEvent>(ChangeAudio, WiSJoy.MessageChannel.audio);
            MessageBus.I.Subscribe<MusicEvent>(PlayMusic, WiSJoy.MessageChannel.audio);
            MessageBus.I.Subscribe<SFXEvent>(Shot, WiSJoy.MessageChannel.audio);
        }

        private void OnDestroy()
        {
            MessageBus.I.Unsubscribe<AudioEvent>(ChangeAudio, WiSJoy.MessageChannel.audio);
            MessageBus.I.Unsubscribe<MusicEvent>(PlayMusic, WiSJoy.MessageChannel.audio);
            MessageBus.I.Unsubscribe<SFXEvent>(Shot, WiSJoy.MessageChannel.audio);
        }
        #endregion

        #region Event
        private void ChangeAudio(AudioEvent audioEvent)
        {
            _masterMixer.SetFloat("MasterVolume", Mathf.Log10(audioEvent.MasterVolume) * 20);
            _musicMixerGroup.SetFloat("MusicVolume", Mathf.Log10(audioEvent.MusicVolume) * 20);
            _sfxMixerGroup.SetFloat("SFXVolume", Mathf.Log10(audioEvent.SFXVolume) * 20);
        }
        #endregion

        #region Music
        private void PlayMusic(MusicEvent musicEvent)
        {
            _musicSource.clip = SO.MusicClips[(int)musicEvent.Key].Clips.Random();
            _musicSource.volume = musicEvent.Volume;
            _musicSource.pitch = musicEvent.Pitch;
            _musicSource.loop = musicEvent.Loop;
            _musicSource.Play();
            if (musicEvent.FadeDuration > 0)
            {
                StartCoroutine(FadeMusic(musicEvent.Volume, musicEvent.FadeDuration));
            }
        }
        // Mix 2 music clips, fade to 0, then fade to target volume
        IEnumerator FadeMusic(float targetVolume, float duration)
        {
            DOVirtual.Float(_musicSource.volume, 0, duration * 0.5f, (x) =>
            {
                _musicSource.volume = x;
            });
            yield return new WaitForSeconds(duration * 0.5f);
            DOVirtual.Float(0, targetVolume, duration * 0.5f, (x) =>
            {
                _musicSource.volume = x;
            });

        }
        #endregion

        #region SFX
        private void Shot(SFXEvent audioEvent)
        {
            _sfxSource.clip = SO.SFXClips[(int)audioEvent.Key].Clips.Random();
            _sfxSource.volume = audioEvent.Volume;
            _sfxSource.pitch = audioEvent.Pitch;
            _sfxSource.Play();
        }
        #endregion
    }

    internal class AudioEvent
    {
        public float MasterVolume = 1f;
        public float MusicVolume = 1f;
        public float SFXVolume = 1f;
    }

    internal class MusicEvent
    {
        public MusicID Key;
        public float Volume = 1f;
        public float Pitch = 1f;
        public bool Loop = false;
        public float FadeDuration = 0.5f;
    }

    internal class SFXEvent
    {
        public SFXID Key;
        public float Volume = 1f;
        public float Pitch = 1f;
    }
}
