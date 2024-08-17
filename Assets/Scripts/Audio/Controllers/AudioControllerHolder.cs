using UnityEngine;

namespace Audio
{
    public interface IAudioControllerHolder
    {
        IAudioController AudioController { get; }
    }

    [CreateAssetMenu(menuName = "ScriptableObject/Audio/AudioControllerHolder")]
    public class AudioControllerHolder : ScriptableObject, IAudioControllerHolder
    {
        [SerializeField] private AudioController _audioController;
        public IAudioController AudioController => _audioController;

        public void Initialize(GameObject audioSourceHolder)
        {
            _audioController.Initialize(audioSourceHolder);
        }
    }
}