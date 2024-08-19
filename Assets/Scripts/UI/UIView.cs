using Management;
using UniTools.Extensions;
using UnityEngine;

namespace UI
{
    public abstract class UIView<T> : MonoBehaviour where T : IGameStateManager
    {
        protected T Model { get; private set; }
        private Canvas _canvas;
        
        public void Initialize(T model)
        {
            Model = model;
            InitializeInternal();

            _canvas = GetComponent<Canvas>();
        }

        protected abstract void InitializeInternal();

        public virtual void ShowScreen() => _canvas.EnableComponent();
        public virtual void HideScreen() => _canvas.DisableComponent();
    }
}