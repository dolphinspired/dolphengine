using System;

namespace DolphEngine.Input.Controls
{
    [Flags]
    public enum Direction
    {
        None =  0,
        Up =    1 >> 1,
        Right = 1 >> 2,
        Down =  1 >> 3,
        Left =  1 >> 4
    }
}
