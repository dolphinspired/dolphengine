namespace DolphEngine
{
    public struct Vector2d
    {
        public Vector2d(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public float X;

        public float Y;

        public static Vector2d Zero = new Vector2d(0, 0);

        public static Vector2d One = new Vector2d(1.000f, 1.000f);

        public override string ToString()
        {
            return $"[ {X}, {Y} ]";
        }
    }
}
