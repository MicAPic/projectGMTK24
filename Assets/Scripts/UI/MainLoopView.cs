using System;
using System.Collections.Generic;
using Management;
using PrimeTween;
using TMPro;
using UniRx;
using UniTools.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainLoopView : UIView<MainLoopManager>
    {
        [SerializeField] private TMP_Text _dayCounterText;
        
        [SerializeField] private TMP_Text _moneyText;
        [SerializeField] private Image _moneyBarFill;
        
        [SerializeField] private RectTransform _heartHolder;
        [SerializeField] private GameObject _heartPrefab;
        
        [SerializeField] private GameObject _pointerIcon;
        
        [Header("Appearance Animation")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TweenSettings<float> _fadeTweenSettings;

        private List<GameObject> _hearts = new List<GameObject>();

        private void Awake()
        {
            _canvasGroup.alpha = _fadeTweenSettings.startValue;
        }

        protected override void InitializeInternal()
        {
            Model.DayCounter.Day.Subscribe(x => _dayCounterText.text = $"{x:00}").AddTo(this);
            
            Model.TreasuryController.Money.Subscribe(x => _moneyText.text = $"L<space=0.1em>{x:0}").AddTo(this);
            Model.TreasuryController.Money.Subscribe(x =>
            {
                Tween.UIFillAmount(_moneyBarFill, x / Model.MaxMoneyAmount, 0.1f);
            }).AddTo(this);

            for (var i = 0; i < Model.MaxHealth; i++)
            {
                var heart = Instantiate(_heartPrefab, _heartHolder);
                _hearts.Add(heart);
            }
            Model.HealthController.Health.Subscribe(i =>
            {
                if (i < _hearts.Count) 
                    _hearts[i].Hide();
            }).AddTo(this);
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