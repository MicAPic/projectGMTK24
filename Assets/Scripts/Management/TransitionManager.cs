using System.Collections;
using Audio;
using Configs;
using PrimeTween;
using UniTools.Patterns.Singletons;
using UnityEngine;
using UnityEngine.SceneManagement;
using AudioType = Audio.AudioType;

namespace Management
{
    public class TransitionManager : PersistentSingleton<TransitionManager>
    {
        [SerializeField] private ConfigurationsHolder _configurations;
        [SerializeField] private Material _transitionMaterial;
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        public void LoadScene(string sceneName)
        {
            _configurations.AudioControllerHolder.AudioController.StopAll(AudioType.BGM);
            _configurations.AudioControllerHolder.AudioController.StopAll(AudioType.SFX);
            _configurations.AudioControllerHolder.AudioController.Play(AudioID.Click);

            Tween.StopAll();
            
            StartCoroutine(PrepareTransition(sceneName));
        }

        private IEnumerator PrepareTransition(string sceneToLoadAfter)
        {
            yield return new WaitForEndOfFrame();

            SetTransitionTexture();

            Time.timeScale = 1.0f; // to prevent bugs
            SceneManager.LoadScene(sceneToLoadAfter);
        }

        private void SetTransitionTexture()
        {
            var screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
            var screenRegion = new Rect(0, 0, Screen.width, Screen.height);
            screenTexture.ReadPixels(screenRegion, 0, 0, false);
            screenTexture.Apply(); // render the texture on GPU
            _transitionMaterial.SetTexture(MainTex, screenTexture);
        }
    }
}