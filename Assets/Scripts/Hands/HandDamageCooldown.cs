using Configs;
using Environment;
using PrimeTween;
using UniRx;
using UniTools.Extensions;
using UnityEngine;
using UnityEngine.U2D;

namespace Hands
{
    public class HandDamageCooldown : MonoBehaviour
    {
        [SerializeField] private HandController _controller;
        [SerializeField] private ConfigurationsHolder _configurations;
        [SerializeField] private SpriteRenderer[] _spritesToFade;
        [SerializeField] private SpriteShapeRenderer _handRenderer;
        [SerializeField] private TweenSettings<float> _fadeSettings;

        private void Start()
        {
            DamagingBehaviour.GotDamaged
                .Where(x => gameObject.CompareTag(x.with))
                .Subscribe(_ => FreezeMovement())
                .AddTo(this);
        }

        private void FreezeMovement()
        {
            var duration = _configurations.MainLoopConfiguration.DamageCooldownDuration;
            var sequence = Sequence.Create()
                .ChainCallback(() => _controller.CanMove = false)
                .ChainDelay(duration)
                .ChainCallback(() => _controller.CanMove = true);
            
            _spritesToFade.ForEach(sprite =>
            {
                sequence.Insert(0.0f, Tween.Alpha(sprite, _fadeSettings.WithDirection(toEndValue: true)));
                sequence.Insert(duration, Tween.Alpha(sprite, _fadeSettings.WithDirection(toEndValue: false)));
            });
            
            sequence.Insert(0.0f, Tween.Custom( 
                _fadeSettings.WithDirection(toEndValue: true), 
                onValueChange: SetAlphaOfHandProper));
            
            sequence.Insert(duration, Tween.Custom( 
                _fadeSettings.WithDirection(toEndValue: false), 
                onValueChange: SetAlphaOfHandProper));
        }

        private void SetAlphaOfHandProper(float alpha)
        {
            var colour = _handRenderer.color;
            _handRenderer.color = new Color(colour.r, colour.g, colour.b, alpha);
        }
    }
}