using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(menuName = "ScriptableObject/Audio/AudioSystems/SoundSystem", fileName = "SoundSystem")]
    public class SoundSystem : AudioSystem
    {
        [SerializeField, Min(1)] private int _maxSpeakersCount = 30;

        private readonly List<AudioSpeaker> _speakers = new List<AudioSpeaker>();

        protected override void DisposeInternal()
        {
            _speakers.ForEach(x => x?.Dispose());
            _speakers.Clear();
        }

        protected override AudioType Type => AudioType.SFX;

        protected override AudioSpeaker GetSpeaker(AudioPriority priority)
        {
            var speaker = _speakers.Find(x => x.IsFree);
            if (speaker != null) return speaker;

            if (_speakers.Count >= _maxSpeakersCount)
            {
                var speakerToOverride = _speakers.FirstOrDefault(x => (int)x.Priority > (int)priority);
                if (speakerToOverride == null) return null;

                speakerToOverride.Dispose();

                return speakerToOverride;
            }

            var newSpeaker = new AudioSpeaker(AddSource(), MixerGroup);

            _speakers.Add(newSpeaker);

            return newSpeaker;
        }
    }
}