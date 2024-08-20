using System;
using System.Collections;
using Audio;
using Configs;
using UI;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using AudioType = Audio.AudioType;

namespace Management
{
    public class GameOverManager : MonoBehaviour, IGameStateManager
    {
        public static IObservable<Unit> GameOverStarted => _gameOverStarted;
        private static Subject<Unit> _gameOverStarted = new Subject<Unit>();
        
        [SerializeField] private GameOverView _loseUi;
        [SerializeField] private GameOverView _winUi;

        private ConfigurationsHolder _configurations;
        private MainLoopManager.MainLoopResult _mainLoopResult;
        
        public int DayCount { get; private set; }

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
        
        public GameOverManager WithDayCount(int dayCount)
        {
            DayCount = dayCount;
            return this;
        }

        public IEnumerator Run()
        {
            Debug.LogWarning($"I'm running! ({this.GetType().Name})");
            Debug.LogWarning($"Result: {_mainLoopResult}");
            
            _gameOverStarted.OnNext(Unit.Default);
            _configurations.AudioControllerHolder.AudioController.StopAll(AudioType.SFX);
            
            switch (_mainLoopResult)
            {
                case MainLoopManager.MainLoopResult.Success:
                    _configurations.AudioControllerHolder.AudioController.Play(AudioID.FanfareShort);
                    _winUi.ShowScreen();
                    break;
                case MainLoopManager.MainLoopResult.Failure:
                    _configurations.AudioControllerHolder.AudioController.Play(AudioID.GameOver);
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
            TransitionManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        public void Return()
        {
            TransitionManager.Instance.LoadScene("Menu");
        }
    }
}
