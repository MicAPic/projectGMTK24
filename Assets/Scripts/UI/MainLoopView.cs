using Management;
using TMPro;
using UniRx;
using UnityEngine;

namespace UI
{
    public class MainLoopView : UIView<MainLoopManager>
    {
        [SerializeField] private TMP_Text _dayCounterText;
        
        protected override void InitializeInternal()
        {
            Model.DayCounter.Day.Subscribe(x => _dayCounterText.text = $"Days: {x}").AddTo(this);
        }
    }
}