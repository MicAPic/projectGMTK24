using System.Collections;
using Audio;
using Configs;
using Environment;
using PrimeTween;
using UniRx;
using UnityEngine;

namespace Hands
{
    public class HandPresenter : MonoBehaviour
    {
        [Header("Damage Animation")]
        [SerializeField] TweenSettings _shrinkTweenSettings;
        [SerializeField] private Transform _palm;
        [SerializeField] private ConfigurationsHolder _configurations;
        

        private Vector3 _defaultPosition;
        private bool _isAnimatingDamage;

        private void Start()
        {
            _defaultPosition = _palm.position;
            
            DamagingBehaviour.GotDamaged
                .Where(x => gameObject.CompareTag(x.with))
                .Subscribe(x => StartCoroutine(AnimateDamage()))
                .AddTo(this);
        }

        private IEnumerator AnimateDamage()
        {
            if (_isAnimatingDamage) yield break;

            _isAnimatingDamage = true;
            _configurations.AudioControllerHolder.AudioController.Play(AudioID.Ouch);
            
            yield return Tween.Position(
                _palm,
                _defaultPosition,
                _shrinkTweenSettings)
                .ToYieldInstruction();
            
            _isAnimatingDamage = false;
        }
    }
}