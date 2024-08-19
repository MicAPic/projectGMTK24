using System.Collections;
using Audio;
using Configs;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Management
{
    public class GameOverManager : MonoBehaviour, IGameStateManager
    {
        [SerializeField] private GameOverView _ui;
        
        private ConfigurationsHolder _configurations;
        private MainLoopManager.MainLoopResult _mainLoopResult;

        public void Initialize(ConfigurationsHolder configurations)
        {
            _configurations = configurations;
            
            _ui.Initialize(this);
            _ui.HideScreen();
        }

        public GameOverManager WithResult(MainLoopManager.MainLoopResult mainLoopResult)
        {
            _mainLoopResult = mainLoopResult;
            return this;
        }

        public IEnumerator Run()
        {
            Debug.LogWarning($"I'm running! ({this.GetType().Name})");
            Debug.LogWarning($"Result: {_mainLoopResult}");
            
            _configurations.AudioControllerHolder.AudioController.Play(AudioID.GameOver);
            _ui.ShowScreen();
            yield return null;
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
