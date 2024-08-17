using System;
using UnityEngine;

namespace Audio
{
    [Serializable]
    public class AudioResource
    {
        [field: SerializeField] public AudioType Type { get; private set; }
        [field: SerializeField] public AudioPriority Priority { get; private set; }
        [field: SerializeField] public bool IsLooped { get; private set; }
        [field: SerializeField, Range(0.0f, 1.0f)] public float Volume { get; private set; } = 1.0f;
        [field: SerializeField, Min(0.0f)] public float FadeInTime { get; private set; }
        [field: SerializeField, Min(0.0f)] public float FadeOutTime { get; private set; }
        [field: SerializeField] public bool RandomPitch { get; private set; }
        [field: SerializeField] public AudioClip AudioClip { get; private set; }
    }
}
