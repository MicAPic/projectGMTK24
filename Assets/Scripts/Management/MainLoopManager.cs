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
        
        private List<Coroutine> _timedCoroutines = new List<Coroutine>();

        public void Initialize(ConfigurationsHolder configurations)
        {
            DayCounter = new DayCounter(configurations);
            
            _ui.Initialize(this);
        }
        
        public IEnumerator Run()
        {
            StartTimedCoroutines();
            
            Debug.LogWarning($"I'm running! ({this.GetType().Name})");
            yield return new WaitForSeconds(1000.0f);
            
            StopTimedCoroutines();
        }

        private void StartTimedCoroutines()
        {
            var dayCounterRoutine = StartCoroutine(DayCounter.Run());
            _timedCoroutines.Add(dayCounterRoutine);
        }

        private void StopTimedCoroutines()
        {
            _timedCoroutines.ForEach(StopCoroutine);
        }
    }
}
