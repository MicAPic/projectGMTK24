using System.Collections;
using Audio;
using Configs;
using PrimeTween;
using UnityEngine;

namespace Management
{
    public class TutorialManager : MonoBehaviour, IGameStateManager
    {
        private ConfigurationsHolder _configurations;

        public void Initialize(ConfigurationsHolder configurations)
        {
            _configurations = configurations;
        }

        public IEnumerator Run()
        {
            Debug.LogWarning($"I'm running! ({this.GetType().Name})");
            _configurations.AudioControllerHolder.AudioController.Play(AudioID.Fanfare);
            _configurations.AudioControllerHolder.AudioController.Play(AudioID.Ambiance);
            
            yield return Tween.Delay(9.0f).ToYieldInstruction();
            
            Debug.LogWarning("Tutorial ended");
        }
    }
}
