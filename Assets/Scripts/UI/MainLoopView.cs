using Management;
using TMPro;
using UniRx;
using UnityEngine;

namespace UI
{
    public class MainLoopView : UIView<MainLoopManager>
    {
        [SerializeField] private TMP_Text _dayCounterText;
        [SerializeField] private TMP_Text _moneyText;
        [SerializeField] private TMP_Text _healthText; // todo: this should be an array of hearts
        [SerializeField] private GameObject _pointerIcon;
        
        protected override void InitializeInternal()
        {
            Model.DayCounter.Day.Subscribe(x => _dayCounterText.text = $"Days: {x}").AddTo(this);
            Model.TreasuryController.Money.Subscribe(x => _moneyText.text = $"Money: {x}").AddTo(this);
            Model.HealthController.Health.Subscribe(x => _healthText.text = $"Health: {x}").AddTo(this);
        }

        public GameObject GetPointerIcon() => _pointerIcon;
    }
}