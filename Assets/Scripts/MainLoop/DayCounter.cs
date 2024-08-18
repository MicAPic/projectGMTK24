using System.Collections;
using Configs;
using PrimeTween;
using UniRx;

namespace MainLoop
{
    public class DayCounter
    {
        private readonly ConfigurationsHolder _configurations;
        
        public IReadOnlyReactiveProperty<int> Day => _day;
        private ReactiveProperty<int> _day = new ReactiveProperty<int>(0);

        public DayCounter(ConfigurationsHolder configurations)
        {
            _configurations = configurations;
        }

        public IEnumerator Run()
        {
            while (true)
            {
                _day.Value++;
                yield return Tween.Delay(_configurations.MainLoopConfiguration.TimeBetweenDays).ToYieldInstruction();
            }
        }
    }
}