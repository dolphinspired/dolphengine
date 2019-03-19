using Microsoft.Xna.Framework;

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
            return new Point(position.X, position.Y);
        }

        public static Vector2 ToVector2(this Position2d position)
        {
            return new Vector2(position.X, position.Y);
        }

        public static Rectangle ToRectangle(this Rect2d rect)
        {
            return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}
