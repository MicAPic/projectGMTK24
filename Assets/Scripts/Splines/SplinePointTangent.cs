using System;

namespace Splines
{
    [Flags]
    public enum SplinePointTangent
    {
        Undefined = 0,
        Left = 1,
        Right = 2
    }
}