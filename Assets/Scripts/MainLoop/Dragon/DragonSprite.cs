using UniRx;
using UnityEngine;

namespace Dragon
{
    public class DragonSprite : MonoBehaviour
    {
        public IReadOnlyReactiveProperty<bool> IsVisible => _isVisible;
        private readonly ReactiveProperty<bool> _isVisible = new(false);
        
        private void OnBecameVisible() => _isVisible.Value = true;
        private void OnBecameInvisible() => _isVisible.Value = false;
    }
}