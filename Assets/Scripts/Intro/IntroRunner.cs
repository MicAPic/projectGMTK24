using System.Collections;
using Management;
using UnityEngine;
using UnityEngine.Playables;

namespace Intro
{
    public class IntroRunner : MonoBehaviour
    {
        private PlayableDirector _cutscene;
        private void Awake()
        {
            _cutscene = GetComponent<PlayableDirector>();
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds((float) _cutscene.duration);
            TransitionManager.Instance.LoadScene("Menu");
        }
    }
}