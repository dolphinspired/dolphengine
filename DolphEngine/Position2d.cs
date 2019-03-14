namespace DolphEngine
{
    public struct Position2d
    {
        public static Position2d Zero = new Position2d(0, 0);

        public Position2d(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X;

        public int Y;

        public override string ToString()
        {
            return $"[ {X}, {Y} ]";
        }
    }
}
