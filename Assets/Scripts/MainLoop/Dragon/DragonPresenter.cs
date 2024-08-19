using System.Collections;
using Audio;
using Configs;
using Environment;
using MainLoop;
using PrimeTween;
using UniRx;
using UniTools.Collections;
using UniTools.Extensions;
using UniTools.Patterns.ObjectPool;
using UniTools.Utils;
using UnityEngine;

namespace Dragon
{
    public class DragonPresenter : MonoBehaviour
    {
        public IReadOnlyReactiveProperty<bool> IsVisible => _sprite.IsVisible;
        
        [SerializeField] private DragonSprite _sprite;
        [SerializeField] private SerializableDictionary<ScreenSpawnSide, AnimationClip> _animationClips;
        [SerializeField] private TweenSettings _travelTweenSettings;
        [SerializeField] private ConfigurationsHolder _configurations;
        [SerializeField] private DamagingBehaviour _fireElementPrefab;
        [SerializeField] private float _fireYPositionOffset;

        private Animation _animation;
        private ScreenSpawnSide _side;
        private ObjectPool<DamagingBehaviour> _fireElementPool;
        private bool _isSpawningFire;
        private IAudioPlayController _dragonSoundPlayController;

        private void Awake()
        {
            _animation = GetComponent<Animation>();
            _fireElementPool = new ObjectPool<DamagingBehaviour>(
                _fireElementPrefab, 
                FireHolder.Instance.transform, 
                10);
        }

        public void SetSide(ScreenSpawnSide spawnSide)
        {
            _side = spawnSide;
            _animation.clip = _animationClips[spawnSide];
        }

        public IEnumerator Run()
        {
            _isSpawningFire = true;
            _dragonSoundPlayController = _configurations.AudioControllerHolder.AudioController.Play(AudioID.Roar);

            StartSpawningFire();
            
            yield return Tween.Position(
                transform,
                transform.position + _configurations.MainLoopConfiguration.DragonTravelDistance[_side],
                _travelTweenSettings)
                .ToYieldInstruction();
            
            _isSpawningFire = false;
            _dragonSoundPlayController.Stop();
        }

        private void StartSpawningFire()
        {
            if (_isSpawningFire is false) return;
            StartCoroutine(SpawnFireRoutine());
            
            Debug.Log(_configurations.MainLoopConfiguration.FireSpawnCooldown);
            Debug.Log(_configurations.MainLoopConfiguration.FireRemainTime);
        }

        private IEnumerator SpawnFireRoutine()
        {
            while (_isSpawningFire)
            {
                yield return Tween.Delay(_configurations.MainLoopConfiguration.FireSpawnCooldown).ToYieldInstruction();
                var (fire, token) = _fireElementPool.Get();

                FireHolder.Instance.StartCoroutine(EndFire(token));

                fire.transform.position = new Vector3(transform.position.x, transform.position.y - _fireYPositionOffset, 0);
                fire.Show();
                yield return null;
            }
        }

        private IEnumerator EndFire(DisposableToken token)
        {
            yield return Tween.Delay(_configurations.MainLoopConfiguration.FireRemainTime).ToYieldInstruction();
            token.Dispose();
        }
    }
}
