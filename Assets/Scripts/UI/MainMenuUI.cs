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
        [Space]
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private MusicSystem _musicSystem;
        [Space]
        [SerializeField] private Slider _soundSlider;
        [SerializeField] private SoundSystem _soundSystem;
        
        [SerializeField] private ConfigurationsHolder _configurations;

        private void Start()
        {
            _startButton.Bind(Restart);

            _musicSlider.value = _musicSystem.GetVolume().ConvertFromLogarithmic();
            _musicSlider.onValueChanged.AddListener(x => _musicSystem.SetVolume(x.ConvertToLogarithmic()));
            
            _soundSlider.value = _soundSystem.GetVolume().ConvertFromLogarithmic();
            _soundSlider.onValueChanged.AddListener(x => _soundSystem.SetVolume(x.ConvertToLogarithmic()));
            
            _configurations.AudioControllerHolder.AudioController.Play(AudioID.MenuBGM);
        }

        private void Restart()
        {
            TransitionManager.Instance.LoadScene("Main");
        }
    }
}