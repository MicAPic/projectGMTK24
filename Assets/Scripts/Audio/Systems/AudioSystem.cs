using System;
using System.Collections;
using System.Collections.Generic;
using Management;
using UniRx;
using UniTools.Extensions;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public abstract class AudioSystem : ScriptableObject, IDisposable
    {
        [SerializeField] private AudioMixerGroup _mixerGroup;
        [SerializeField] private string _mixerVolumeKey = "Volume";
        [SerializeField] private float _mutedMixerValue = -80.0f;
        [SerializeField] private float _defaultMixerValue = 0.0f;

        private readonly HashSet<AudioSpeaker> _speakers = new HashSet<AudioSpeaker>();

        protected abstract AudioType Type { get; }
        
        private GameObject _audioSourceHolder;

        protected AudioMixerGroup MixerGroup => _mixerGroup;

        protected abstract AudioSpeaker GetSpeaker(AudioPriority priority);
        protected abstract void DisposeInternal();

        public void Initialize(GameObject audioSourceHolder)
        {
            _audioSourceHolder = audioSourceHolder;
            SetVolume(AudioManager.Instance.Volumes[Type]);
        }

        protected AudioSource AddSource()
        {
            return _audioSourceHolder.AddComponent<AudioSource>();
        }

        public void Mute()
        {
            MixerGroup.audioMixer.SetFloat(_mixerVolumeKey, _mutedMixerValue);
        }

        public void Unmute()
        {
            MixerGroup.audioMixer.SetFloat(_mixerVolumeKey, _defaultMixerValue);
        }
        
        public void SetVolume(float volume)
        {
            MixerGroup.audioMixer.SetFloat(_mixerVolumeKey, volume);
            AudioManager.Instance.Volumes[Type] = volume;
        }
        
        public float GetVolume()
        {
            return AudioManager.Instance.Volumes[Type];
        }

        public void StopAll() => _speakers.ForEach(x => Observable.FromCoroutine(x.Stop).Subscribe());
        public IAudioPlayController Play(AudioResource audioResource) => RunPlayRoutine(PlayRoutine, audioResource);

        private IAudioPlayController RunPlayRoutine<TAudio>(Func<TAudio, AudioPlayController,IEnumerator> routine, TAudio audioResource)
        {
            var playController = new AudioPlayController();
            var routineDisposable = Observable.FromCoroutine(() => routine(audioResource, playController)).Subscribe(_ => playController?.Dispose());
            playController.Initialize(routineDisposable);
            return playController;
        }

        private IEnumerator PlayRoutine(AudioResource audioResource, AudioPlayController playController)
        {
            var speaker = GetSpeaker(audioResource.Priority);
            if (speaker == null)
            {
                Debug.LogWarning($"Can't play audio '{audioResource.AudioClip.name}': have no available speaker".Highlight());
                yield break;
            }

            speaker.SetBusy();
            _speakers.Add(speaker);
            playController.Initialize(speaker);
            
            yield return speaker.Stop();
            yield return speaker.Play(audioResource);

            speaker.Dispose();
        }

        public void Dispose()
        {
            _speakers.ForEach(x => x?.Dispose());
            DisposeInternal();
        }
    }
} 