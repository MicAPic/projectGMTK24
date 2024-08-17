using System.Collections;
using UniTools.Patterns.Singletons;
using UnityEngine;

namespace Management
{
    public class GameOverManager : Singleton<GameOverManager>, IGameStateManager
    {
        public IEnumerator Run()
        {
            yield return null;
            Debug.LogWarning($"I'm running! ({nameof(this.GetType)})");
        }
    }
}
