using UniRx;
using UnityEngine;

namespace Hands
{
    public class HandsPresenter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private HandController _controller;

        private void Start()
        {
            // _controller.DirectionLeft
            //     .Where(x => x.sqrMagnitude > 0)
            //     .Subscribe()
        }
    }
}