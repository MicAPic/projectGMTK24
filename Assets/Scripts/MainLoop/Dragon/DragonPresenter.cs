using System.Collections;
using Configs;
using Environment;
using MainLoop;
using PrimeTween;
using UniTools.Collections;
using UniTools.Extensions;
using UniTools.Patterns.ObjectPool;
using UniTools.Utils;
using UnityEngine;

namespace Dragon
{
    public class DragonPresenter : MonoBehaviour
    {
        public bool IsVisible { get; private set; }
        
        [SerializeField] private SerializableDictionary<ScreenSpawnSide, AnimationClip> _animationClips;
        [SerializeField] private TweenSettings _travelTweenSettings;
        [SerializeField] private ConfigurationsHolder _configurations;
        [SerializeField] private DamagingBehaviour _fireElementPrefab;

        private Animation _animation;
        private ScreenSpawnSide _side;
        private ObjectPool<DamagingBehaviour> _fireElementPool;
        private bool _isSpawningFire;

        private void Awake()
        {
            _animation = GetComponent<Animation>();
            _fireElementPool = new ObjectPool<DamagingBehaviour>(
                _fireElementPrefab, 
                FireHolder.Instance.transform, 
                10);
        }

        private void OnBecameVisible() => IsVisible = true;
        private void OnBecameInvisible() => IsVisible = false;

        public void SetSide(ScreenSpawnSide spawnSide)
        {
            _side = spawnSide;
            _animation.clip = _animationClips[spawnSide];
        }

        public IEnumerator Run()
        {
            _isSpawningFire = true;

            StartSpawningFire();
            
            yield return Tween.Position(
                transform,
                transform.position + _configurations.MainLoopConfiguration.DragonTravelDistance[_side],
                _travelTweenSettings)
                .ToYieldInstruction();
            
            _isSpawningFire = false;
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
                
                fire.transform.position = transform.position;
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
