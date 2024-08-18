using System.Collections;
using Audio;
using Configs;
using Houses;
using UniRx;
using UnityEngine;

namespace Management
{
    public class TutorialManager : MonoBehaviour, IGameStateManager
    {
        private ConfigurationsHolder _configurations;
        private bool _isActive;

        public void Initialize(ConfigurationsHolder configurations)
        {
            _configurations = configurations;
            
            _isActive = PlayerPrefs.GetInt(_configurations.MainLoopConfiguration.TutorialPrefsKey, 0) == 0;

#if UNITY_EDITOR
            if (_configurations.MainLoopConfiguration.TutorialAlwaysStarts) _isActive = true;
#endif
        }

        public IEnumerator Run()
        {
            Debug.LogWarning($"I'm running! ({this.GetType().Name})");
            _configurations.AudioControllerHolder.AudioController.Play(AudioID.Ambiance);
            
            if (_isActive is false) yield break;
            
            _configurations.AudioControllerHolder.AudioController.Play(AudioID.Fanfare);

            var collectEventSubscription = HouseController.CollectedCoins
                .Subscribe(x => _isActive = false);
            
            while (_isActive)
            {
                yield return null;
            }
            
            collectEventSubscription?.Dispose();
            PlayerPrefs.SetInt(_configurations.MainLoopConfiguration.TutorialPrefsKey, 1);
            Debug.LogWarning("Tutorial ended");
        }
    }
}
