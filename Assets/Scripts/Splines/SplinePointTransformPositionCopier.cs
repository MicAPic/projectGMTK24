using UniRx;
using UnityEngine;
using UnityEngine.U2D;

namespace Splines
{
    public class SplinePointTransformPositionCopier : MonoBehaviour
    {
        [SerializeField] private SpriteShapeController _spriteShape;
        [SerializeField] private int _pointIndex;
        [SerializeField] private Transform _from;
        [SerializeField] private Vector3 _offset;

        private Spline _spline;

        private void Awake()
        {
            _spline = _spriteShape.spline;

            _from
                .ObserveEveryValueChanged(x => x.position)
                .Subscribe(CopyPositionWithOffset)
                .AddTo(this);
        }

        private void CopyPositionWithOffset(Vector3 pos)
        {
            _spline.SetPosition(_pointIndex, transform.InverseTransformPoint(pos) + _offset);
        }
    }
}