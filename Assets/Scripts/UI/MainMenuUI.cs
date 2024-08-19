using Audio;
using Configs;
using UnityEngine;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private ConfigurationsHolder _configurations;

        private void Start()
        {
            _configurations.AudioControllerHolder.AudioController.Play(AudioID.MenuBGM);
        }
    }
}