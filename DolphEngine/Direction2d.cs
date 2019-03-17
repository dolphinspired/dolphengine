using System;

namespace DolphEngine
{
    [Flags]
    public enum Direction2d
    {
        None        = 0,

        Up          = 1 << 1,
        Right       = 1 << 2,
        Down        = 1 << 3,
        Left        = 1 << 4,

        UpRight     = Up | Right,
        DownRight   = Down | Right,
        DownLeft    = Down | Left,
        UpLeft      = Up | Left
    }
}
