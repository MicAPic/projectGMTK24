using System.Collections;
using Configs;
using Environment;
using PrimeTween;
using UniRx;
using UnityEngine;

namespace Hands
{
    public class HandDamageCooldown : MonoBehaviour
    {
        [SerializeField] private HandController _controller;
        [SerializeField] private ConfigurationsHolder _configurations;

        private void Start()
        {
            DamagingBehaviour.GotDamaged
                .Where(x => gameObject.CompareTag(x.with))
                .Subscribe(_ => StartCoroutine(FreezeMovement()))
                .AddTo(this);
        }

        private IEnumerator FreezeMovement()
        {
            _controller.CanMove = false;
            yield return Tween.Delay(_configurations.MainLoopConfiguration.DamageCooldownDuration).ToYieldInstruction();
            _controller.CanMove = true;
        }
    }
}