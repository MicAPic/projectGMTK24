using Configs;
using Environment;
using Houses;
using System.Collections;
using UniRx;
using UnityEngine;

namespace MainLoop
{
    public class HealthController
    {
        private readonly ConfigurationsHolder _configurations;

        public IReadOnlyReactiveProperty<int> Health => _health;
        private ReactiveProperty<int> _health = new ReactiveProperty<int>(0);

        public HealthController(ConfigurationsHolder configurations)
        {
            _configurations = configurations;
            _health.Value = _configurations.MainLoopConfiguration.MaxHealth;

            DamagingBehaviour.GotDamaged.Subscribe(_ => RemoveHeart());
        }

        private void RemoveHeart() => _health.Value = --_health.Value;

        public void ResetHearts() => _health.Value = _configurations.MainLoopConfiguration.MaxHealth;    
    }
}