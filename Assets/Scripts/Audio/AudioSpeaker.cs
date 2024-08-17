using System;
using System.Collections;
using UniTools.Extensions;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class AudioSpeaker : IDisposable
    {
        private const float PitchRandom = 0.17f;

        private readonly AudioSource _source;

        private float _fadeOutTime = 0f;

        public AudioPriority Priority { get; private set; }
        public bool IsBusy { get; private set; }
        public bool IsFree => IsBusy is false;


        public AudioSpeaker(AudioSource source, AudioMixerGroup mixerGroup)
        {
            _source = source;
            _source.playOnAwake = false;
            _source.loop = false;
            _source.outputAudioMixerGroup = mixerGroup;
        }


        public void SetBusy() => IsBusy = true;
        
        public void SetVolume(float volume) => _source.volume = volume;
        public void Mute() => _source.DisableComponent();
        public void Unmute() => _source.EnableComponent();

        public IEnumerator Play(AudioResource audioResource)
        {
            Priority = audioResource.Priority;
            _fadeOutTime = audioResource.FadeOutTime;

            if (_source == null)
            {
                Debug.LogWarning($"Audio source is null or destroyed, can't play audio {audioResource.AudioClip}");
                yield break;
            }

            _source.EnableComponent();
            _source.clip = audioResource.AudioClip;
            _source.pitch = audioResource.RandomPitch ? 1 - PitchRandom + UnityEngine.Random.value * PitchRandom * 2 : 1;
            _source.loop = audioResource.IsLooped;
            _source.volume = 0.0f;

            _source.Play();

            yield return FadeVolumeRoutine(_source.volume, audioResource.Volume, audioResource.FadeInTime);

            if (audioResource.IsLooped)
            {
                while (true)
                    yield return null;
            }

            yield return null;
            while (_source != null && _source.clip != null && _source.time < _source.clip.length && _source.time > float.Epsilon)
            {
                yield return null;
            }
        }
        
        public void Pause() => _source.Pause();
        public void Resume() => _source.UnPause();

        public IEnumerator Stop()
        {
            if (_source == null)
            {
                yield break;
            }

            if (_source.isPlaying is false)
            {
                yield break;
            }

            _source.loop = false;
            yield return FadeVolumeRoutine(_source.volume, 0.0f, _fadeOutTime);
            _source.Stop();
            _source.DisableComponent();
        }

        private IEnumerator FadeVolumeRoutine(float initialVolume, float finalVolume, float fadeTime)
        {
            var timePassed = 0.0f;
            while (timePassed < fadeTime)
            {
                _source.volume = Mathf.Lerp(initialVolume, finalVolume, timePassed / fadeTime);

                yield return null;
                timePassed += Time.deltaTime;
            }

            _source.volume = finalVolume;
        }

        public void Dispose()
        {
            if (_source == null) 
            {
                return;
            }

            if (_source.isPlaying) _source.Stop();

            _source.volume = 1f;
            _source.pitch = 1f;
            _source.clip = null;
            _source.mute = false;
            _source.loop = false;

            IsBusy = false;
        }
    }
}
