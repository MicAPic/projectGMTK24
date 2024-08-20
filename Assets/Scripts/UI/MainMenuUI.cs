using System;
using System.Collections;
using Audio;
using Configs;
using Management;
using PrimeTween;
using TMPro;
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
        [Space]
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TweenSettings<float> _titleFadeSettings;
        [SerializeField] private float _titleAppearanceDelay = 1.0f;
        
        [SerializeField] private ConfigurationsHolder _configurations;

        private IEnumerator Start()
        {
            _title.Hide();
            _startButton.Bind(StartGame);
            BindWithSound(_openSettingsButton, OpenScroll);
            BindWithSound(_closeSettingsButton, CloseScroll);

            _musicSlider.value = _musicSystem.GetVolume().ConvertFromLogarithmic();
            _musicSlider.onValueChanged.AddListener(x => _musicSystem.SetVolume(x.ConvertToLogarithmic()));
            
            _soundSlider.value = _soundSystem.GetVolume().ConvertFromLogarithmic();
            _soundSlider.onValueChanged.AddListener(x => _soundSystem.SetVolume(x.ConvertToLogarithmic()));
            
            _configurations.AudioControllerHolder.AudioController.Play(AudioID.MenuBGM);

            yield return new WaitForSeconds(_titleAppearanceDelay);
            _title.Show();
        }

        private void OpenScroll()
        {
            _scrollWindow.Open();
            Tween.Alpha(_title, _titleFadeSettings);
        }

        private void CloseScroll()
        {
            _scrollWindow.Close()
                .Chain(Tween.Alpha(_title, _titleFadeSettings.WithDirection(false)));
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