using System;
using System.Collections;
using Hands;
using UniRx;
using UnityEngine;

namespace Houses
{
    public class HouseController : MonoBehaviour
    {
        [Header("Logic")]
        [SerializeField]
        private float containerLimit;
        [SerializeField]
        private float collectingSpeed;
        [SerializeField]
        private float rechargingSpeed;
        [SerializeField]
        private BoxCollider2D boxCollider;

        [Header("View")]
        [SerializeField]
        private Transform mask;

        [SerializeField]
        private Vector3 endMaskPosition;

        private float _currentContainerValue;
        private bool _isHandTriggered = false;
        private bool _isBothHandsTriggered = false;
        private bool _resetHouse = false;
        private Vector3 _defaultMaskPosition;

        public static IObservable<float> CollectedCoins => _collectedCoins;
        private static Subject<float> _collectedCoins = new Subject<float>();

        private void Awake()
        {
            _defaultMaskPosition = mask.localPosition;
        }

        // Start is called before the first frame update
        void Start()
        {
            _currentContainerValue = containerLimit;
            StartCoroutine(Run());
        }

        // Update is called once per frame
        void Update()
        {
            if (_resetHouse)
            {
                _resetHouse = false;
                StartCoroutine(Run());
            }

            mask.localPosition = Vector3.Lerp(endMaskPosition, _defaultMaskPosition, _currentContainerValue / containerLimit);
        }

        public IEnumerator Run()
        {
            yield return Collecting();
            yield return Recharging();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.TryGetComponent(out HandController handController))
            {
                if (_isHandTriggered)
                    _isBothHandsTriggered = true;
                else
                    _isHandTriggered = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out HandController handController))
            {
                if(_isBothHandsTriggered)
                    _isBothHandsTriggered = false;
                else
                    _isHandTriggered = false;
            }
        }

        private IEnumerator Collecting()
        {
            while(_currentContainerValue > 0)
            {
                while (_isHandTriggered)
                {
                    if (_currentContainerValue <= 0)
                        break;

                    var collectedCoins = collectingSpeed * Time.deltaTime;
                    _currentContainerValue -= collectedCoins;
                    _collectedCoins.OnNext(collectedCoins);

                    yield return null;
                }
                while(_currentContainerValue <= 0 && _isHandTriggered)
                {
                    yield return null;
                }
                yield return null;
            }
        }

        private IEnumerator Recharging()
        {
            while (_currentContainerValue < containerLimit)
            {
                _currentContainerValue += rechargingSpeed * Time.deltaTime;
                yield return null;
            }
            _currentContainerValue = containerLimit;
            _resetHouse = true;
        }

    }
}