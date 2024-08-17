using UniTools.Collections;
using UnityEngine;

namespace Audio
{
    public interface IAudioLibrary
    {
        bool TryGetAudio(AudioID audioID, out AudioResource audioResource);
    }
    
    public class NullAudioHolder : IAudioLibrary
    {
        bool IAudioLibrary.TryGetAudio(AudioID audioID, out AudioResource audioResource)
        {
            audioResource = null;
            return false;
        }
    }
    
    [CreateAssetMenu(menuName = "ScriptableObject/Audio/AudioLibrary")]
    public class AudioLibrary : ScriptableObject, IAudioLibrary
    {
        [SerializeField] private SerializableDictionary<AudioID, AudioResource> _audios;

        public bool TryGetAudio(AudioID audioID, out AudioResource audioResource) => _audios.TryGetValue(audioID, out audioResource);
    }
}
