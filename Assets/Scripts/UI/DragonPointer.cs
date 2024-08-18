using Dragon;
using PrimeTween;
using UniRx;
using UniTools.Extensions;
using UnityEngine;

namespace UI
{
    public class DragonPointer : MonoBehaviour
    {
        [SerializeField] private float _pointerImageSize;

        private UnityEngine.Camera _camera;
        private GameObject _pointerIcon;
        private Vector3 _cameraCenterPosition;

        public void Initialize(DragonPresenter presenter, GameObject pointer)
        {
            _camera = UnityEngine.Camera.main;
            _pointerIcon = pointer;

            presenter.IsVisible.Where(x => x).Subscribe(_ => _pointerIcon.Hide()).AddTo(this);
            presenter.IsVisible.Where(x => x is false).Subscribe(_ => _pointerIcon.Show()).AddTo(this);

            presenter
                .ObserveEveryValueChanged(x => x.gameObject.activeInHierarchy)
                .Subscribe(x => Tween.Delay(0.1f, onComplete: () => { _pointerIcon.SetActive(x);}))
                .AddTo(this);
        }

        void Update()
        {
            _cameraCenterPosition = new Vector3(_camera.transform.position.x, _camera.transform.position.y, 0);
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);

            Vector3 fromCenterToDragon = transform.position - new Vector3(_camera.transform.position.x, _camera.transform.position.y, 0);
            Ray ray = new Ray(_cameraCenterPosition, fromCenterToDragon);
            Debug.DrawRay(_cameraCenterPosition, fromCenterToDragon, Color.red);

            float rayMinDistance = Mathf.Infinity;

            for (int p = 0; p < 4; p++)
            {
                if (planes[p].Raycast(ray, out float distance))
                {
                    if (distance < rayMinDistance)
                    {
                        rayMinDistance = distance;
                    }
                }
            }

            rayMinDistance = Mathf.Clamp(rayMinDistance, 0, fromCenterToDragon.magnitude);
            Vector3 worldPosition = ray.GetPoint(rayMinDistance);
            Vector3 position = _camera.WorldToScreenPoint(worldPosition);
            position = new Vector3((Mathf.Abs(position.x - _pointerImageSize / 2)) * Mathf.Sign(position.x),
                (Mathf.Abs(position.y - _pointerImageSize / 2)) * Mathf.Sign(position.y), 0);

            _pointerIcon.transform.position = position;
        }
    }
}
