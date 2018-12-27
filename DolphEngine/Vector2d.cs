namespace DolphEngine
{
    public class Vector2d
    {
        public static Vector2d Zero = new Vector2d(0, 0);

        public Vector2d(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public float X;

        public float Y;
    }
}
