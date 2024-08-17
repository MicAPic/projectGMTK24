using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private List<Transform> handsPositions;
    [SerializeField]
    private Transform background;
    [SerializeField]
    private float deltaBoundries = 2;

    private float _startCameraYPosition;
    private float _endCameraYPosition;

    private float _startZoomValue = 5;
    private float _endZoomValue = 8;


    private Camera _camera;
    private float _maxDistanceBetweenHandAndCastle;

    private Vector3 _castlePosition;


    void Start()
    {
        _camera = GetComponent<Camera>();
        _camera.orthographicSize = _startZoomValue;
        _castlePosition = new Vector3(background.position.x, background.position.y - background.lossyScale.y / 2, 0);
        Vector3 backgroundAngle = new Vector3(background.position.x + background.lossyScale.x / 2 - deltaBoundries, 
                                            background.position.y - background.lossyScale.y / 2 - deltaBoundries, 0);

        _maxDistanceBetweenHandAndCastle = Vector3.Distance(_castlePosition, backgroundAngle);

        _startCameraYPosition = background.position.y - 3;
        _endCameraYPosition = background.position.y;
        transform.position = new Vector3(background.position.x, _startCameraYPosition, -10);
    }

    void LateUpdate()
    {
        float currentDistanceBetweenHandsAndCastle = Mathf.Max(
            Vector3.Distance(handsPositions[0].position, _castlePosition),
            Vector3.Distance(handsPositions[1].position, _castlePosition));
        float currentZoom = Mathf.Lerp(_startZoomValue, _endZoomValue, currentDistanceBetweenHandsAndCastle / _maxDistanceBetweenHandAndCastle);
        _camera.orthographicSize = currentZoom;

        float currentYPosition = Mathf.Lerp(_startCameraYPosition, _endCameraYPosition, currentZoom/_endZoomValue);
        Debug.Log(currentYPosition);
        transform.position = new Vector3(background.position.x, currentYPosition, -10);
    }
}
