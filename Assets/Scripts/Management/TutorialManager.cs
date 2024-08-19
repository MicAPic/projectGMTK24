using System.Collections;
using Audio;
using Configs;
using Houses;
using UI;
using UniRx;
using UnityEngine;

namespace Management
{
    public class TutorialManager : MonoBehaviour, IGameStateManager
    {
        [SerializeField] private TutorialView _ui;

        private ConfigurationsHolder _configurations;
        private bool _isActive;

        public void Initialize(ConfigurationsHolder configurations)
        {
            _configurations = configurations;
            
            _isActive = PlayerPrefs.GetInt(_configurations.MainLoopConfiguration.TutorialPrefsKey, 0) == 0;

#if UNITY_EDITOR
            if (_configurations.MainLoopConfiguration.TutorialAlwaysStarts) _isActive = true;
#endif
            
            _ui.Initialize(this);
            _ui.HideScreen();
        }

        public IEnumerator Run()
        {
            Debug.LogWarning($"I'm running! ({this.GetType().Name})");
            _configurations.AudioControllerHolder.AudioController.Play(AudioID.Ambiance);
            
            if (_isActive is false)
            {
                _configurations.AudioControllerHolder.AudioController.Play(AudioID.FanfareShort);
                yield return new WaitForSeconds(_configurations.MainLoopConfiguration.DelayBeforeStart);
                yield break;
            }
            
            _ui.ShowScreen();
            
            var fanfarePlayer = 
                _configurations.AudioControllerHolder.AudioController.Play(AudioID.Fanfare);

            var collectEventSubscription = HouseController.CollectedCoins
                .Subscribe(x => _isActive = false);
            
            while (_isActive)
            {
                yield return null;
            }
            
            fanfarePlayer.Stop();
            collectEventSubscription?.Dispose();
            PlayerPrefs.SetInt(_configurations.MainLoopConfiguration.TutorialPrefsKey, 1);
            Debug.LogWarning("Tutorial ended");
            _ui.HideScreen();
        }
    }
}
