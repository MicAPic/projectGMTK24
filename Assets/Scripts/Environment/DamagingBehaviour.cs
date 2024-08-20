using System;
using UniRx;
using UniTools.Extensions;
using UnityEngine;

namespace Environment
{
    public class DamagingBehaviour : MonoBehaviour
    {
        public static IObservable<(string with, Vector3 at)> GotDamaged => _gotDamaged;
        private static readonly Subject<(string, Vector3)> _gotDamaged = new Subject<(string, Vector3)>();

        private float _cooldawnTime = 3;

        private static float _startLeftTime = 0;
        private static float _startRightTime = 0;

        private static float ElapsedLeftHandTime => Time.time - _startLeftTime;
        private static float ElapsedRightHandTime => Time.time - _startRightTime;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("LeftHand") && (ElapsedLeftHandTime > _cooldawnTime || _startLeftTime == 0))
            {
                _startLeftTime = Time.time;
                _gotDamaged.OnNext((col.tag, transform.position));
                this.Hide();
            }

            if (col.CompareTag("RightHand") && (ElapsedRightHandTime > _cooldawnTime || _startRightTime == 0))
            {
                _startRightTime = Time.time;
                _gotDamaged.OnNext((col.tag, transform.position));
                this.Hide();
            }
        }
    }
}