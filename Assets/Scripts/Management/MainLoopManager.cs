using System.Collections;
using System.Collections.Generic;
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

        public void Initialize(ConfigurationsHolder configurations)
        {
            DayCounter = new DayCounter(configurations);
            TreasuryController = new TreasuryController(configurations);

            var dragon = Instantiate(configurations.MainLoopConfiguration.DragonPrefab);
            DragonSpawner = new DragonSpawner(configurations, dragon, _ui.GetPointerIcon());
            
            _ui.Initialize(this);
        }
        
        public IEnumerator Run()
        {
            Debug.LogWarning($"I'm running! ({this.GetType().Name})");
            StartTimedCoroutines();
            
            yield return new WaitWhile(() => TreasuryController.Money.Value > 0.0f);
            
            StopTimedCoroutines();
        }

        private void StartTimedCoroutines()
        {
            var dayCounterRoutine = StartCoroutine(DayCounter.Run());
            _timedCoroutines.Add(dayCounterRoutine);
            
            var moneyDecrementRoutine = StartCoroutine(TreasuryController.Run());
            _timedCoroutines.Add(moneyDecrementRoutine);
            
            var dragonSpawnRoutine = StartCoroutine(DragonSpawner.Run());
            _timedCoroutines.Add(dragonSpawnRoutine);
        }

        private void StopTimedCoroutines() => _timedCoroutines.ForEach(StopCoroutine);
    }
}
