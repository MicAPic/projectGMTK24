using Hands;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    [Header("Logic")]
    [SerializeField]
    private float containerLimit;
    [SerializeField]
    private float collectingSpeed;
    [SerializeField]
    private float rechargingSpeed;

    [Header("Logic")]
    [SerializeField]
    private Transform mask;

    [SerializeField]
    private Vector3 endMaskPosition;

    private float _currentContainerValue;
    private bool _isHandTriggered = false;
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
            _isHandTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out HandController handController))
        {
            _isHandTriggered = false;
        }
    }

    private IEnumerator Collecting()
    {
        while(_currentContainerValue > 0)
        {
            while (_isHandTriggered)
            {
                var collectedCoins = collectingSpeed * Time.deltaTime;
                _currentContainerValue -= collectedCoins;
                _collectedCoins.OnNext(collectedCoins);
                if (_currentContainerValue < 0)
                {
                    _currentContainerValue = 0;
                    yield break;
                }

                yield return null;
            }
            yield return null;
        }
    }

    private IEnumerator Recharging()
    {
        while (_currentContainerValue < containerLimit)
        {
            _currentContainerValue += collectingSpeed * Time.deltaTime;
            yield return null;
        }
        _currentContainerValue = containerLimit;
        _resetHouse = true;
    }

}
