using Management;
using UnityEngine;

namespace UI
{
    public abstract class UIView<T> : MonoBehaviour where T : IGameStateManager
    {
        protected T Model { get; private set; }
        
        public void Initialize(T model)
        {
            Model = model;
            InitializeInternal();
        }

        protected abstract void InitializeInternal();
    }
}