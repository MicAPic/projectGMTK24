using System;
using Audio;
using Configs;
using Management;
using UniTools.Extensions;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _openSettingsButton;
        [SerializeField] private Button _closeSettingsButton;
        [Space]
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private MusicSystem _musicSystem;
        [Space]
        [SerializeField] private Slider _soundSlider;
        [SerializeField] private SoundSystem _soundSystem;
        [Space]
        [SerializeField] private ScrollWindow _scrollWindow;
        
        [SerializeField] private ConfigurationsHolder _configurations;

        private void Start()
        {
            _startButton.Bind(StartGame);
            BindWithSound(_openSettingsButton, _scrollWindow.Open);
            BindWithSound(_closeSettingsButton, _scrollWindow.Close);

            _musicSlider.value = _musicSystem.GetVolume().ConvertFromLogarithmic();
            _musicSlider.onValueChanged.AddListener(x => _musicSystem.SetVolume(x.ConvertToLogarithmic()));
            
            _soundSlider.value = _soundSystem.GetVolume().ConvertFromLogarithmic();
            _soundSlider.onValueChanged.AddListener(x => _soundSystem.SetVolume(x.ConvertToLogarithmic()));
            
            _configurations.AudioControllerHolder.AudioController.Play(AudioID.MenuBGM);
        }

        private void BindWithSound(Button button, Action onClick)
        {
            button.Bind(() =>
            {
                onClick?.Invoke();
                _configurations.AudioControllerHolder.AudioController.Play(AudioID.Click);
            });
        }

        private void StartGame()
        {
            TransitionManager.Instance.LoadScene("Main");
        }
    }
}