using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public struct Line
    {
        [field: SerializeField] public Vector3 Start { get; private set; }
        [field: SerializeField] public Vector3 End { get; private set; }
    }
}