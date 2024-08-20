using System.Collections.Generic;
using Configs;
using UniTools.Patterns.Singletons;
using UnityEngine;
using AudioType = Audio.AudioType;

namespace Management
{
    public class AudioManager : PersistentSingleton<AudioManager>
    {
        public Dictionary<AudioType, float> Volumes { get; }= new()
        {
            {AudioType.SFX, 0.994f},
            {AudioType.BGM, 0.994f},
        };

        [SerializeField] private ConfigurationsHolder _configurations;
        
        protected override void AwakeInternal()
        {
            _configurations.AudioControllerHolder.Initialize(gameObject);
        }
    }
}