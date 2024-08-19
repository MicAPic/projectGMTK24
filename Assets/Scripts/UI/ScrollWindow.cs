using System;
using System.Collections;
using PrimeTween;
using UnityEngine;

namespace UI
{
    public class ScrollWindow : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _scrollBody;
        [SerializeField] private TweenSettings<float> _alphaTweenSettings;
        [SerializeField] private TweenSettings<Vector2> _sizeTweenSettings;

        private void Awake()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = _alphaTweenSettings.startValue;
            _scrollBody.sizeDelta = _sizeTweenSettings.startValue;
        }

        public void Open()
        {
            Sequence.Create()
                .Chain(Tween.Alpha(_canvasGroup, _alphaTweenSettings))
                .Chain(Tween.UISizeDelta(_scrollBody, _sizeTweenSettings))
                .ChainCallback(() => _canvasGroup.interactable = true);
        }
        
        public void Close()
        {
            Sequence.Create()
                .ChainCallback(() => _canvasGroup.interactable = false)
                .Chain(Tween.UISizeDelta(_scrollBody, _sizeTweenSettings.WithDirection(false)))
                .Chain(Tween.Alpha(_canvasGroup, _alphaTweenSettings.WithDirection(false)));
        }
    }
}