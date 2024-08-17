using System;
using System.Collections.Generic;
using System.Linq;
using UniTools.Collections;
using UniTools.Extensions;
using UnityEngine;


namespace Audio
{
    public interface IAudioController : IDisposable
    {
        IAudioLibrary Library { get; }
        void Initialize(GameObject audioSourceHolder);

        bool IsDefined(AudioType type);
        IAudioPlayController Play(AudioResource audioResource);
        IAudioPlayController Play(AudioID audioID);

        void StopAll(AudioType type);
        void MuteAll(AudioType type);
        void UnmuteAll(AudioType type);
    }

    [Serializable]
    public class AudioController : IAudioController
    {
        [SerializeField] private SerializableDictionary<AudioType, AudioSystem> _audioSystems;
        [SerializeField] private AudioLibrary _library;

        public IAudioLibrary Library => _library;

        public void Initialize(GameObject audioSourceHolder)
        {
            _audioSystems.Values.ForEach(x => x.Initialize(audioSourceHolder));
        }

        public bool IsDefined(AudioType type) => _audioSystems.All(x => type.HasFlag(x.Key));
        public void StopAll(AudioType type) => GetAudioSystems(type)?.ForEach(x => x.StopAll());
        public void MuteAll(AudioType type) => GetAudioSystems(type)?.ForEach(x => x.Mute());
        public void UnmuteAll(AudioType type) => GetAudioSystems(type)?.ForEach(x => x.Unmute());
        
        private IEnumerable<AudioSystem> GetAudioSystems(AudioType type)
        {
            var found = false;
            foreach (var entry in _audioSystems)
            {
                if (type.HasFlag(entry.Key))
                {
                    found = true;
                    yield return entry.Value;
                }
            }

            if (found is false)
                Debug.LogError($"Can't find audio system for audio type \"{type}\"");
        }

        public IAudioPlayController Play(AudioID audioID)
        {
            if (audioID == AudioID.Undefined)
                return new NullAudioPlayController();

            if (Library.TryGetAudio(audioID, out var audioResource))
                return Play(audioResource);
            
            Debug.LogError($"Can't play audio \"{audioID}\": not found!");
            return new NullAudioPlayController();
        }

        public IAudioPlayController Play(AudioResource audioResource)
        {
            var system = GetAudioSystems(audioResource.Type).FirstOrDefault();
            return system == default ? new NullAudioPlayController() : system.Play(audioResource);
        }

        public void Dispose()
        {
            foreach (var system in _audioSystems)
            {
                system.Value.Dispose();
            }
        }
    }
}