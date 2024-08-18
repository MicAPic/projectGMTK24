using Environment;
using PrimeTween;
using UniRx;
using UnityEngine;

namespace Camera
{
    public class CameraPresenter : MonoBehaviour
    {
        [Header("Camera Shake")]
        [SerializeField] private float _cameraShakeStrength = 1.0f;
        [SerializeField] private float _cameraShakeDuration = 0.5f;
        [SerializeField] private float _cameraShakeFrequency = 10.0f;

        private UnityEngine.Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<UnityEngine.Camera>();
        }

        private void Start()
        {
            DamagingBehaviour.GotDamaged.Subscribe(_ => ShakeCamera()).AddTo(this);
        }

        private void ShakeCamera()
        {
            Tween.ShakeCamera(
                _camera, 
                _cameraShakeStrength,
                _cameraShakeDuration,
                _cameraShakeFrequency);
        }
    }
}