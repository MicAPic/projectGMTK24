using System;
using Management;
using PrimeTween;
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
        
        [Header("Appearance Animation")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TweenSettings<float> _fadeTweenSettings;

        private void Awake()
        {
            _canvasGroup.alpha = _fadeTweenSettings.startValue;
        }

        protected override void InitializeInternal()
        {
            Model.DayCounter.Day.Subscribe(x => _dayCounterText.text = $"{x:00}").AddTo(this);
            Model.TreasuryController.Money.Subscribe(x => _moneyText.text = $"Money: {x}").AddTo(this);
            Model.HealthController.Health.Subscribe(x => _healthText.text = $"Health: {x}").AddTo(this);
        }

        public GameObject GetPointerIcon() => _pointerIcon;

        public override void ShowScreen()
        {
            base.ShowScreen();
            Tween.Alpha(_canvasGroup, _fadeTweenSettings);
        }

        public override void HideScreen()
        {
            Tween.Alpha(_canvasGroup, _fadeTweenSettings.WithDirection(false))
                .OnComplete(() => base.HideScreen());
        }
    }
}