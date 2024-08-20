using System.Collections;
using Audio;
using Configs;
using UniTools.Patterns.Singletons;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TransitionEffect : MonoBehaviour
    {
        [SerializeField] private ConfigurationsHolder _configurations;
        [SerializeField] private float transitionDuration = 1.0f;
        private Material _material;
        private static readonly int Progress = Shader.PropertyToID("flip_time");

        void Awake()
        {
            _material = GetComponent<RawImage>().material;
        }

        // Start is called before the first frame update
        void Start()
        {
            _configurations.AudioControllerHolder.AudioController.Play(AudioID.PageTurn);
            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            _material.SetFloat(Progress, 0.99f);
            yield return new WaitForEndOfFrame();
            
            for (var t = 0.0f; t <= 1.0f; t += Time.deltaTime / transitionDuration)
            {
                var progress = Mathf.Lerp(0.0f, 0.99f, t);
                _material.SetFloat(Progress, progress);
                yield return null;
            }
            _material.SetFloat(Progress, 0.99f);
            
            GetComponent<RawImage>().raycastTarget = false;
        }

    }
}
