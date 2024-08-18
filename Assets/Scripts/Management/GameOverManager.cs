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

        public void Initialize(ConfigurationsHolder configurations)
        {
            _configurations = configurations;
            
            _ui.Initialize(this);
            _ui.HideScreen();
        }

        public IEnumerator Run()
        {
            Debug.LogWarning($"I'm running! ({this.GetType().Name})");
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
