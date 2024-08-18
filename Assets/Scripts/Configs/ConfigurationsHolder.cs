using Audio;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "ScriptableObject/Configs/ConfigurationsHolder", order = 0)]
    public class ConfigurationsHolder : ScriptableObject
    {
        [field: SerializeField] public AudioControllerHolder AudioControllerHolder { get; private set; }
        [field: SerializeField] public MainLoopConfiguration MainLoopConfiguration { get; private set; }
    }
}