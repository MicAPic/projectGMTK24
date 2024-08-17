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
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("LeftHand") || col.CompareTag("RightHand"))
            {
                _gotDamaged.OnNext((col.tag, transform.position));
                this.Hide();
            }
        }
    }
}