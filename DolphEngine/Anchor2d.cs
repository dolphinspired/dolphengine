using System;

namespace DolphEngine
{
    [Flags]
    public enum Anchor2d
    {
        Default         = 0b0000,
        Left            = 0b0001,
        Right           = 0b0010,
        Center          = 0b0011,
        Top             = 0b0100,
        TopLeft         = Top | Left,
        TopRight        = Top | Right,
        TopCenter       = Top | Center,
        Bottom          = 0b1000,
        BottomLeft      = Bottom | Left,
        BottomRight     = Bottom | Right,
        BottomCenter    = Bottom | Center,
        Middle          = 0b1100,
        MiddleLeft      = Middle | Left,
        MiddleRight     = Middle | Right,
        MiddleCenter    = Middle | Center,
    }
}
