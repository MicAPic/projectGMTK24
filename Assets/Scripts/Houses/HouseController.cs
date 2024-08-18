using Hands;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    [SerializeField]
    private float containerLimit;

    [SerializeField]
    private float collectingSpeed;

    [SerializeField]
    private float rechargingSpeed;

    private float _currentContainerValue;
    private bool _isHandTriggered = false;
    private bool _resetHouse = false;

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
    }

    public IEnumerator Run()
    {
        yield return StartCoroutine(Collecting());
        yield return StartCoroutine(Recharging());
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
                _currentContainerValue -= collectingSpeed * Time.deltaTime;
                if(_currentContainerValue < 0)
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
