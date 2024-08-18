using UnityEngine;

namespace Utils
{
    public static class RandomUtils
    {
        public static Vector3 GetRandomPoint(this Line line)
        {
            var rand = Random.Range(0.0f, 1.0f);
            var result = rand * Vector3.Normalize(line.End - line.Start) + line.Start;
            return result;
        }
    }
}