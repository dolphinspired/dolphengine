namespace DolphEngine
{
    public struct Position2d
    {
        public static Position2d Zero = new Position2d(0, 0);

        public Position2d(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public float X;

        public float Y;

        public override string ToString()
        {
            return $"[ {X}, {Y} ]";
        }
    }
}
