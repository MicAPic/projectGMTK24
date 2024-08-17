using System.Collections;
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
            
            yield return Tween.Position(
                _palm,
                _defaultPosition,
                _shrinkTweenSettings)
                .ToYieldInstruction();
            
            _isAnimatingDamage = false;
        }
    }
}