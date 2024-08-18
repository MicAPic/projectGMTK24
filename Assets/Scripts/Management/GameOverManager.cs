using System.Collections;
using Audio;
using Configs;
using UnityEngine;

namespace Management
{
    public class GameOverManager : MonoBehaviour, IGameStateManager
    {
        private ConfigurationsHolder _configurations;

        public void Initialize(ConfigurationsHolder configurations)
        {
            _configurations = configurations;
        }

        public IEnumerator Run()
        {
            Debug.LogWarning($"I'm running! ({this.GetType().Name})");
            _configurations.AudioControllerHolder.AudioController.Play(AudioID.GameOver);
            yield return null;
        }
    }
}
