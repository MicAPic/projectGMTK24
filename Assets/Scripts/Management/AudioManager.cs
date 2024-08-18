using Configs;
using UniTools.Patterns.Singletons;
using UnityEngine;

namespace Management
{
    public class AudioManager : PersistentSingleton<AudioManager>
    {
        [SerializeField] private ConfigurationsHolder _configurations;
        
        protected override void AwakeInternal()
        {
            _configurations.AudioControllerHolder.Initialize(gameObject);
        }
    }
}