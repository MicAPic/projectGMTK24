using System;
using System.Collections;
using Audio;
using Configs;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using AudioType = Audio.AudioType;

namespace Management
{
    public class GameOverManager : MonoBehaviour, IGameStateManager
    {
        [SerializeField] private GameOverView _loseUi;
        [SerializeField] private GameOverView _winUi;

        private ConfigurationsHolder _configurations;
        private MainLoopManager.MainLoopResult _mainLoopResult;

        public void Initialize(ConfigurationsHolder configurations)
        {
            _configurations = configurations;
            
            _loseUi.Initialize(this);
            _loseUi.HideScreen();

            _winUi.Initialize(this);
            _winUi.HideScreen();
        }

        public GameOverManager WithResult(MainLoopManager.MainLoopResult mainLoopResult)
        {
            _mainLoopResult = mainLoopResult;
            return this;
        }

        public IEnumerator Run()
        {
            Debug.LogWarning($"I'm running! ({this.GetType().Name})");
            Debug.LogWarning($"Result: {_mainLoopResult}");
            
            _configurations.AudioControllerHolder.AudioController.StopAll(AudioType.SFX);
            _configurations.AudioControllerHolder.AudioController.Play(AudioID.GameOver);
            
            switch (_mainLoopResult)
            {
                case MainLoopManager.MainLoopResult.Success:
                    _winUi.ShowScreen();
                    break;
                case MainLoopManager.MainLoopResult.Failure:
                    _loseUi.ShowScreen();
                    break;
                case MainLoopManager.MainLoopResult.Undefined:
                default:
                    throw new Exception($"Main Loop result is {_mainLoopResult}");
            }
            yield return null;
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
