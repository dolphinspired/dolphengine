﻿using Microsoft.Xna.Framework;

namespace DolphEngine.MonoGame
{
    public static class StructExtensions
    {
        public static Vector2 ToVector2(this Vector2d vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        public static Point ToPoint(this Position2d position)
        {
            return new Point((int)position.X, (int)position.Y);
        }

        public static Point ToPoint(this Size2d size)
        {
            return new Point((int)size.Width, (int)size.Height);
        }

        public static Vector2 ToVector2(this Position2d position)
        {
            return new Vector2(position.X, position.Y);
        }

        public static Rectangle ToRectangle(this Rect2d rect)
        {
            return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }

        public static Color ToColor(this ColorRGBA color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }
    }
}
