using UnityEngine;

namespace Hands
{
    public class HandPositionConstraints : MonoBehaviour
    {
        [field: SerializeField] public Vector2 MaxPosition { get; private set; }
        [field: SerializeField] public Vector2 MinPosition { get; private set; }
    }
}