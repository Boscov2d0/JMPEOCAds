using System;

namespace Code.Gameplay.AI.Data
{
    [Flags]
    public enum RoutePointFlags : byte
    {
        None = 0,
        Jump = 1 << 0,
        Wait = 1 << 1,
        Noise = 1 << 2,
        Fall = 1 << 3
    }
}
