using UnityEngine;

namespace Utils
{
    public static class MathUtils
    {
        public static float ConvertFromLogarithmic(this float value)
        {
            return Mathf.Pow(10, value / 20.0f);
        }
        
        public static float ConvertToLogarithmic(this float value)
        {
            return Mathf.Log10(value) * 20.0f;
        }
    }
}