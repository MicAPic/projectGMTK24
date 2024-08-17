using System.Collections.Generic;
using UnityEngine;

namespace Camera
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private List<Transform> handsPositions;
        [SerializeField] private SpriteRenderer background;
        [SerializeField] private float deltaBoundries = 2;

        private float _startCameraYPosition;
        private float _endCameraYPosition;

        private float _startZoomValue = 5;
        private float _endZoomValue = 8;


        private UnityEngine.Camera _camera;
        private float _maxDistanceBetweenHandAndCastle;

        private Vector3 _castlePosition;


        void Start()
        {
            _camera = GetComponent<UnityEngine.Camera>();
            _camera.orthographicSize = _startZoomValue;

            var backgroundPosition = background.transform.position;
            var size = background.size;

            _castlePosition = new Vector3(backgroundPosition.x, backgroundPosition.y - size.y / 2, 0);
            Vector3 backgroundAngle = new Vector3(
                backgroundPosition.x + size.x / 2 - deltaBoundries, 
                backgroundPosition.y - size.y / 2 - deltaBoundries, 
                0);

            _maxDistanceBetweenHandAndCastle = Vector3.Distance(_castlePosition, backgroundAngle);

            _startCameraYPosition = backgroundPosition.y - 3;
            _endCameraYPosition = backgroundPosition.y;
            transform.position = new Vector3(backgroundPosition.x, _startCameraYPosition, -10);
        }

        void LateUpdate()
        {
            float currentDistanceBetweenHandsAndCastle = Mathf.Max(
                Vector3.Distance(handsPositions[0].position, _castlePosition),
                Vector3.Distance(handsPositions[1].position, _castlePosition));

            var progress = currentDistanceBetweenHandsAndCastle / _maxDistanceBetweenHandAndCastle;
            
            float currentZoom = Mathf.Lerp(_startZoomValue, _endZoomValue, progress);
            _camera.orthographicSize = currentZoom;

            float currentYPosition = Mathf.Lerp(_startCameraYPosition, _endCameraYPosition, progress);
            transform.position = new Vector3(background.transform.position.x, currentYPosition, -10);
        }
    }
}
