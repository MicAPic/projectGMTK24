using System.Collections;
using Configs;
using Houses;
using UniRx;
using UnityEngine;

namespace MainLoop
{
    public class TreasuryController
    {
        private readonly ConfigurationsHolder _configurations;
        
        private float _startTime;
        
        public IReadOnlyReactiveProperty<float> Money => _money;
        private ReactiveProperty<float> _money = new ReactiveProperty<float>(0);

        private float ElapsedTime => Time.time - _startTime;
        private AnimationCurve DifficultyCurve => _configurations.MainLoopConfiguration.MoneyReductionSpeedCurve;
        private float MaxMoneyAmount => _configurations.MainLoopConfiguration.MaxMoneyAmount;
        private float MinMoneyAmount => _configurations.MainLoopConfiguration.MinMoneyAmount;

        public TreasuryController(ConfigurationsHolder configurations)
        {
            _configurations = configurations;
            _money.Value = _configurations.MainLoopConfiguration.MaxMoneyAmount;
            
            ResetTime();

            HouseController.CollectedCoins.Subscribe(AddMoney);
        }

        public IEnumerator Run()
        {
            while (true)
            {
                var currentDecrement = DifficultyCurve.Evaluate(ElapsedTime);
                
                RemoveMoney(currentDecrement * Time.deltaTime);
                yield return null;
            }
        }

        public void ResetTime() => _startTime = Time.time;

        private void AddMoney(float value) => _money.Value = Mathf.Min(MaxMoneyAmount, _money.Value + value);
        private void RemoveMoney(float value) => _money.Value = Mathf.Max(MinMoneyAmount, _money.Value - value);
    }
}