using System;
using System.Collections;
using System.Collections.Generic;
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
        private readonly float _startTime;
        private readonly DragonPresenter _dragon;
        
        private float ElapsedTime => Time.time - _startTime;
        private AnimationCurve CooldownCurve => _configurations.MainLoopConfiguration.TimeBetweenDragons;
        private AnimationCurve DelayCurve => _configurations.MainLoopConfiguration.DragonStartDelay;
        private IReadOnlyDictionary<ScreenSpawnSide, Line> DragonSpawnRanges => _configurations.MainLoopConfiguration.DragonSpawnRanges;

        public DragonSpawner(ConfigurationsHolder configurations, GameObject dragon, GameObject pointer)
        {
            _configurations = configurations;
            
            _startTime = Time.time;
            
            _dragon = dragon.GetComponent<DragonPresenter>();
            dragon.GetComponent<DragonPointer>().Initialize(_dragon, pointer);
            
            _dragon.Hide();
        }
        
        public IEnumerator Run()
        {
            while (true)
            {
                var currentCooldown = CooldownCurve.Evaluate(ElapsedTime);
                yield return Tween.Delay(currentCooldown).ToYieldInstruction();
                
                SetDragonStartPosition();
                // TODO: warn player with an !
                
                var currentStartDelay = DelayCurve.Evaluate(ElapsedTime);
                yield return Tween.Delay(currentStartDelay).ToYieldInstruction();
                
                yield return _dragon.Run();
                
                _dragon.Hide();
            }
        }

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