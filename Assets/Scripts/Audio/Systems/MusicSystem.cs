using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(menuName = "ScriptableObject/Audio/AudioSystems/MusicSystem", fileName = "MusicSystem")]
    public class MusicSystem : AudioSystem
    {
        private readonly Dictionary<AudioPriority, AudioSpeaker> _speakers = new Dictionary<AudioPriority, AudioSpeaker>();

        protected override string VolumePrefsKey => "MUSIC_VOLUME";

        protected override AudioSpeaker GetSpeaker(AudioPriority priority)
        {
            if (_speakers.ContainsKey(priority) is false)
            {
                _speakers[priority] = new AudioSpeaker(AddSource(), MixerGroup);
            }

            return _speakers[priority];
        }

        protected override void DisposeInternal()
        {
            foreach (var speaker in _speakers.Values)
            {
                speaker?.Dispose();
            }

            _speakers.Clear();
        }
    }
}