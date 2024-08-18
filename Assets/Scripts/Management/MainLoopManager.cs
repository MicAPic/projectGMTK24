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
        [SerializeField] private MainLoopView _ui;
        
        public DayCounter DayCounter { get; private set; }
        public TreasuryController TreasuryController { get; private set; }
        public DragonSpawner DragonSpawner { get; private set; }
        
        private List<Coroutine> _timedCoroutines = new List<Coroutine>();

        private ConfigurationsHolder _configurations;
        private IAudioPlayController _mainMusicPlayController;

        public void Initialize(ConfigurationsHolder configurations)
        {
            _configurations = configurations;
            
            DayCounter = new DayCounter(configurations);
            TreasuryController = new TreasuryController(configurations);

            var dragon = Instantiate(configurations.MainLoopConfiguration.DragonPrefab);
            DragonSpawner = new DragonSpawner(configurations, dragon, _ui.GetPointerIcon());
            
            _ui.Initialize(this);
        }
        
        public IEnumerator Run()
        {
            Debug.LogWarning($"I'm running! ({this.GetType().Name})");
            _mainMusicPlayController = _configurations.AudioControllerHolder.AudioController.Play(AudioID.GameplayBGM);

            StartTimedCoroutines();
            
            yield return new WaitWhile(() => TreasuryController.Money.Value > 0.0f);
            
            StopTimedCoroutines();
            _mainMusicPlayController.Stop();
        }

        private void StartTimedCoroutines()
        {
            var dayCounterRoutine = StartCoroutine(DayCounter.Run());
            _timedCoroutines.Add(dayCounterRoutine);
            
            TreasuryController.ResetTime();
            var moneyDecrementRoutine = StartCoroutine(TreasuryController.Run());
            _timedCoroutines.Add(moneyDecrementRoutine);
            
            DragonSpawner.ResetTime();
            var dragonSpawnRoutine = StartCoroutine(DragonSpawner.Run());
            _timedCoroutines.Add(dragonSpawnRoutine);
        }

        private void StopTimedCoroutines() => _timedCoroutines.ForEach(StopCoroutine);
    }
}
