using MainLoop;
using UniTools.Collections;
using UnityEngine;
using Utils;

namespace Configs
{
    [CreateAssetMenu(menuName = "ScriptableObject/Configs/MainLoopConfiguration", order = 1)]
    public class MainLoopConfiguration : ScriptableObject
    {
        [field: SerializeField] public float DelayBeforeStart { get; private set; } = 3.0f;
        
        [field: SerializeField] public float TimeBetweenDays { get; private set; } = 10.0f;

        [field: SerializeField] public float InitialMoneyAmount { get; private set; } = 333.0f;
        [field: SerializeField] public float MaxMoneyAmount { get; private set; } = 999.0f;
        [field: SerializeField] public float MinMoneyAmount { get; private set; } = 0.0f;
        [field: SerializeField] public AnimationCurve MoneyReductionSpeedCurve { get; private set; }
        
        [field: SerializeField] public float DamageCooldownDuration { get; private set; }
        
        [field: SerializeField] public AnimationCurve DragonStartDelay { get; private set; }
        [field: SerializeField] public AnimationCurve TimeBetweenDragons { get; private set; }
        [field: SerializeField] public SerializableDictionary<ScreenSpawnSide, Line> DragonSpawnRanges { get; private set; }
        [field: SerializeField] public SerializableDictionary<ScreenSpawnSide, Vector3> DragonTravelDistance { get; private set; }
        [field: SerializeField] public GameObject DragonPrefab { get; private set; }
        
        [field: SerializeField] public float FireSpawnCooldown { get; private set; }
        [field: SerializeField] public float FireRemainTime { get; private set; }

        [field: SerializeField] public bool TutorialAlwaysStarts { get; private set; } = false;
        [field: SerializeField] public string TutorialPrefsKey { get; private set; } = "TutorialTimesSeen";

        [field: SerializeField] public int MaxHealth { get; private set; } = 3;
    }
}