using UnityEngine;
using Random = System.Random;

namespace Utils
{
    public static class RandomUtils
    {
        private static readonly Random Rand = new Random();
        
        public static Vector3 GetRandomPoint(this Line line)
        {
            var rand = (float)Rand.NextDouble();
            var result = rand * Vector3.Normalize(line.End - line.Start) + line.Start;
            return result;
        }
    }
}