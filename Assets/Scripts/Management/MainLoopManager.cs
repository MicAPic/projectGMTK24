using System.Collections;
using System.Collections.Generic;
using Audio;
using Configs;
using MainLoop;
using UI;
using UnityEngine;

namespace Management
{
    public class MainLoopManager : MonoBehaviour, IGameStateManager
    {
        public enum MainLoopResult
        {
            Undefined = 0,
            Success = 1,
            Failure = 2
        }
        
        [SerializeField] private MainLoopView _ui;
        
        public MainLoopResult Result { get; private set; }
        
        public DayCounter DayCounter { get; private set; }
        public TreasuryController TreasuryController { get; private set; }
        public HealthController HealthController { get; private set; }
        public DragonSpawner DragonSpawner { get; private set; }
        
        private List<Coroutine> _timedCoroutines = new List<Coroutine>();

        private ConfigurationsHolder _configurations;
        private IAudioPlayController _mainMusicPlayController;

        public void Initialize(ConfigurationsHolder configurations)
        {
            _configurations = configurations;
            
            DayCounter = new DayCounter(configurations);
            TreasuryController = new TreasuryController(configurations);
            HealthController = new HealthController(configurations);

            var dragon = Instantiate(configurations.MainLoopConfiguration.DragonPrefab);
            DragonSpawner = new DragonSpawner(configurations, dragon, _ui.GetPointerIcon());
            
            _ui.Initialize(this);
            _ui.HideScreen();
        }
        
        public IEnumerator Run()
        {
            Debug.LogWarning($"I'm running! ({this.GetType().Name})");
            _mainMusicPlayController = _configurations.AudioControllerHolder.AudioController.Play(AudioID.GameplayBGM);
            _ui.ShowScreen();

            StartTimedCoroutines();

            yield return new WaitWhile(() => TreasuryController.Money.Value < _configurations.MainLoopConfiguration.MaxMoneyAmount && 
                                                HealthController.Health.Value > 0);

            Result = MainLoopResult.Success;

            StopTimedCoroutines();
            _mainMusicPlayController.Stop();
            _ui.HideScreen();
        }

        private void StartTimedCoroutines()
        {
            var dayCounterRoutine = StartCoroutine(DayCounter.Run());
            _timedCoroutines.Add(dayCounterRoutine);

            HealthController.ResetHearts();
            
            DragonSpawner.ResetTime();
            var dragonSpawnRoutine = StartCoroutine(DragonSpawner.Run());
            _timedCoroutines.Add(dragonSpawnRoutine);
        }

        private void StopTimedCoroutines() => _timedCoroutines.ForEach(StopCoroutine);
    }
}
