using System;
using System.Collections;
using Audio;
using Configs;
using Hands;
using PrimeTween;
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

        [Header("View")]
        [SerializeField]
        private Transform mask;
        [SerializeField]
        private SpriteRenderer houseSprite;
        [SerializeField]
        private float spriteRechargeAlpha = 0.1f;

        [SerializeField]
        private Vector3 endMaskPosition;
        
        [Space]
        
        [SerializeField] private ConfigurationsHolder _configurations;

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
            IAudioPlayController coinsSoundPlayController = new NullAudioPlayController();
            
            while (_currentContainerValue > 0)
            {
                if (_isHandTriggered)
                    coinsSoundPlayController = _configurations.AudioControllerHolder.AudioController.Play(AudioID.Coins);

                while (_isHandTriggered)
                {
                    if (_currentContainerValue <= 0)
                    {
                        coinsSoundPlayController.Stop();
                        break;
                    }

                    var collectedCoins = collectingSpeed * Time.deltaTime;
                    _currentContainerValue -= collectedCoins;
                    _collectedCoins.OnNext(collectedCoins);

                    yield return null;
                }
                
                coinsSoundPlayController.Stop();
                
                while (_currentContainerValue <= 0 && _isHandTriggered)
                {
                    yield return null;
                }
                
                yield return null;
            }
        }

        private IEnumerator Recharging()
        {
            Tween.Alpha(houseSprite, spriteRechargeAlpha, 0.1f);
            while (_currentContainerValue < containerLimit)
            {
                _currentContainerValue += rechargingSpeed * Time.deltaTime;
                yield return null;
            }
            _currentContainerValue = containerLimit;
            Tween.Alpha(houseSprite, 1.0f, 0.1f);
            
            _resetHouse = true;
        }

    }
}