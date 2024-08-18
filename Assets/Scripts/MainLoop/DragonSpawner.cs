using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Configs;
using Dragon;
using PrimeTween;
using UI;
using UniTools.Extensions;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace MainLoop
{
    public class DragonSpawner
    {
        private readonly ConfigurationsHolder _configurations;
        private readonly DragonPresenter _dragon;
        
        private float _startTime;

        private float ElapsedTime => Time.time - _startTime;
        private AnimationCurve CooldownCurve => _configurations.MainLoopConfiguration.TimeBetweenDragons;
        private AnimationCurve DelayCurve => _configurations.MainLoopConfiguration.DragonStartDelay;
        private IReadOnlyDictionary<ScreenSpawnSide, Line> DragonSpawnRanges => _configurations.MainLoopConfiguration.DragonSpawnRanges;

        public DragonSpawner(ConfigurationsHolder configurations, GameObject dragon, GameObject pointer)
        {
            _configurations = configurations;
            
            ResetTime();
            
            _dragon = dragon.GetComponent<DragonPresenter>();
            dragon.GetComponent<DragonPointer>().Initialize(_dragon, pointer);
            
            _dragon.Hide();
        }
        
        public IEnumerator Run()
        {
            while (true)
            {
                var currentCooldown = CooldownCurve.Evaluate(ElapsedTime);
                yield return new WaitForSeconds(currentCooldown);
                
                SetDragonStartPosition();
                _configurations.AudioControllerHolder.AudioController.Play(AudioID.Danger);
                
                var currentStartDelay = DelayCurve.Evaluate(ElapsedTime);
                Debug.Log($"Current delay: {currentStartDelay}");
                yield return new WaitForSeconds(currentStartDelay);
                Debug.Log($"Delay is over");
                
                yield return _dragon.Run();
                
                _dragon.Hide();
            }
        }

        public void ResetTime() => _startTime = Time.time;

        private void SetDragonStartPosition()
        {
            var allSides = Enum.GetValues(typeof(ScreenSpawnSide));
            var randomSide = (ScreenSpawnSide)allSides.GetValue(Random.Range(0, allSides.Length));

            var randomPointOnSide = DragonSpawnRanges[randomSide].GetRandomPoint();

            _dragon.SetSide(randomSide);
            _dragon.transform.position = randomPointOnSide;
            
            _dragon.Show();
        }
    }
}