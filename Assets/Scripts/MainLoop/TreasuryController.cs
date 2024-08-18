using System.Collections;
using Configs;
using UniRx;
using UnityEngine;

namespace MainLoop
{
    public class TreasuryController
    {
        private readonly ConfigurationsHolder _configurations;
        private readonly float _startTime;
        
        public IReadOnlyReactiveProperty<float> Money => _money;
        private ReactiveProperty<float> _money = new ReactiveProperty<float>(0);

        private float ElapsedTime => Time.time - _startTime;
        private AnimationCurve DifficultyCurve => _configurations.MainLoopConfiguration.MoneyReductionSpeedCurve;
        private float MaxMoneyAmount => _configurations.MainLoopConfiguration.MaxMoneyAmount;
        private float MinMoneyAmount => _configurations.MainLoopConfiguration.MinMoneyAmount;

        public TreasuryController(ConfigurationsHolder configurations)
        {
            _configurations = configurations;
            _startTime = Time.time;
            _money.Value = _configurations.MainLoopConfiguration.MaxMoneyAmount;

            //TODO: subscribe to static event in houses, call AddMoney()
            HouseController.CollectedCoins.Subscribe(x => AddMoney(x));
        }
        
        public IEnumerator Run()
        {
            while (true)
            {
                var currentDecrement = DifficultyCurve.Evaluate(ElapsedTime);
                // Debug.Log(currentDecrement);
                
                RemoveMoney(currentDecrement);
                yield return null;
            }
        }

        private void AddMoney(float value) => _money.Value = Mathf.Min(MaxMoneyAmount, _money.Value + value);
        private void RemoveMoney(float value) => _money.Value = Mathf.Max(MinMoneyAmount, _money.Value - value);
    }
}