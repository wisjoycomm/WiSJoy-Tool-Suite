using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using WiSJoy.DesignPattern;

namespace WiSJoy.Manager.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;

        [SerializeField] private AudioMixer _masterMixer;
        [SerializeField] private AudioMixer _musicMixerGroup;
        [SerializeField] private AudioMixer _sfxMixerGroup;

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

        private void ChangeAudio(AudioEvent audioEvent)
        {
            _masterMixer.SetFloat("MasterVolume", Mathf.Log10(audioEvent.MasterVolume) * 20);
            _musicMixerGroup.SetFloat("MusicVolume", Mathf.Log10(audioEvent.MusicVolume) * 20);
            _sfxMixerGroup.SetFloat("SFXVolume", Mathf.Log10(audioEvent.SFXVolume) * 20);
        }
        private void PlayMusic(MusicEvent musicEvent)
        {
            _musicSource.volume = musicEvent.Volume;
            _musicSource.pitch = musicEvent.Pitch;
            _musicSource.loop = musicEvent.Loop;
            _musicSource.Play();
        }

        private void Shot(SFXEvent audioEvent)
        {
            _sfxSource.volume = audioEvent.Volume;
            _sfxSource.pitch = audioEvent.Pitch;
            _sfxSource.Play();
        }
    }

    internal class AudioEvent
    {
        public float MasterVolume = 1f;
        public float MusicVolume = 1f;
        public float SFXVolume = 1f;
    }

    internal class MusicEvent
    {
        public string Name;
        public float Volume = 1f;
        public float Pitch = 1f;
        public bool Loop = false;
        public float FadeDuration = 0.5f;
    }

    internal class SFXEvent
    {
        public string Name;
        public float Volume = 1f;
        public float Pitch = 1f;
    }
}
