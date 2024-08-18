using MainLoop;
using UniTools.Collections;
using UnityEngine;

namespace Dragon
{
    public class DragonPresenter : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<ScreenSpawnSide, AnimationClip> _animationClips;
        private Animation _animation;

        private void Awake()
        {
            _animation = GetComponent<Animation>();
        }

        public void SetAnimation(ScreenSpawnSide spawnSide)
        {
            _animation.clip = _animationClips[spawnSide];
        }
    }
}
