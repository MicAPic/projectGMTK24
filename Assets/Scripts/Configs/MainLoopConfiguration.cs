using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "ScriptableObject/Configs/MainLoopConfiguration", order = 1)]
    public class MainLoopConfiguration : ScriptableObject
    {
        [field: SerializeField] public float TimeBetweenDays { get; private set; } = 10.0f;
    }
}