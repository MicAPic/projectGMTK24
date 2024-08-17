using System.Collections;
using Configs;
using UnityEngine;

namespace Management
{
    public class GameOverManager : MonoBehaviour, IGameStateManager
    {
        public void Initialize(ConfigurationsHolder configuration)
        {
            
        }

        public IEnumerator Run()
        {
            yield return null;
            Debug.LogWarning($"I'm running! ({this.GetType().Name})");
        }
    }
}
