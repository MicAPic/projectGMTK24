using UniRx;
using UnityEngine;
using UnityEngine.U2D;

namespace Splines
{
    public class SplinePointOffsetKeeper : MonoBehaviour
    {
        [SerializeField] private SpriteShapeController _spriteShape;
        [SerializeField] private int _from;
        [SerializeField] private int _to;

        private void Awake()
        {
            var spline = _spriteShape.spline;
            var offset = spline.GetPosition(_from) - spline.GetPosition(_to);
            
            spline
                .ObserveEveryValueChanged(x => x.GetPosition(_from))
                .Subscribe(x => spline.SetPosition(_to, x - offset))
                .AddTo(this);
        }
    }
}