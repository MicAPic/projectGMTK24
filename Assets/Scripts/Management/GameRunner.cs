using System.Collections;
using Configs;
using UniTools.Patterns.Singletons;
using UnityEngine;

namespace Management
{
    public class GameRunner : Singleton<GameRunner>
    {
        [SerializeField] private ConfigurationsHolder _configurations;

        [Header("Managers")]
        [SerializeField] private TutorialManager _tutorialManager;
        [SerializeField] private MainLoopManager _mainLoopManager;
        [SerializeField] private GameOverManager _gameOverManager;

        protected override void AwakeInternal()
        {
            _tutorialManager.Initialize(_configurations);
            _mainLoopManager.Initialize(_configurations);
            _gameOverManager.Initialize(_configurations);
            
            _configurations.AudioControllerHolder.Initialize(gameObject);
        }

        // Start is called before the first frame update
        private IEnumerator Start()
        {
            yield return RunGame();
        }

        private IEnumerator RunGame()
        {
            yield return _tutorialManager.Run();
            yield return _mainLoopManager.Run();
            yield return _gameOverManager.Run();
        }
    }
}
