using System.Collections;
using Configs;
using UnityEngine;

namespace Management
{
    public class TutorialManager : MonoBehaviour, IGameStateManager
    {
        public void Initialize(ConfigurationsHolder configuration)
        {
            
        }

        public IEnumerator Run()
        {
            Debug.LogWarning($"I'm running! ({this.GetType().Name})");
            yield return null;
        }
    }
}
