﻿namespace DolphEngine.Eco.Components
{
    public class PositionComponent2d : Component
    {
        public PositionComponent2d() : this(0, 0) { }

        public PositionComponent2d(int x, int y)
        {
            this.Set(x, y);
        }

        public int X;

        public int Y;

        public void Set(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
