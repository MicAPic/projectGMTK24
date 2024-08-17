using System.Collections;
using UniTools.Patterns.Singletons;
using UnityEngine;

namespace Management
{
    public class GameManager : Singleton<GameManager>
    {
        // Start is called before the first frame update
        private IEnumerator Start()
        {
            yield return RunGame();
        }

        private IEnumerator RunGame()
        {
            yield return null;
            Debug.LogWarning("I'm running!");
        }
    }
}
