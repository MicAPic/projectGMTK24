using UniRx;
using UnityEngine;
using UnityEngine.U2D;

namespace Splines
{
    public class SplinePointRotateTowards : MonoBehaviour
    {
        [SerializeField] private SpriteShapeController _spriteShape;
        [SerializeField] private int _index;
        [SerializeField] private int _towards;
        [SerializeField] private SplinePointTangent _tangent;

        private Spline _spline;
        
        private void Awake()
        {
            _spline = _spriteShape.spline;

            // _spline
            //     .ObserveEveryValueChanged(x => x.GetPosition(_towards))
            //     .Subscribe(RotateTowards)
            //     .AddTo(this);
        }

        private void RotateTowards(Vector3 targetPosition)
        {
            var currentPosition = _spline.GetPosition(_index);
            var direction = (targetPosition - currentPosition).normalized;
            
            if (_tangent.HasFlag(SplinePointTangent.Left))
                _spline.SetLeftTangent(_index, direction);
            if (_tangent.HasFlag(SplinePointTangent.Right))
                _spline.SetRightTangent(_index, direction);

        }
    }
}