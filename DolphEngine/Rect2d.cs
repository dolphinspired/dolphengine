namespace DolphEngine
{
    public struct Rect2d
    {
        public Rect2d(Rect2d rect) : this(rect.X, rect.Y, rect.Width, rect.Height) { }

        public Rect2d(Position2d position, Size2d size) : this(position.X, position.Y, size.Width, size.Height) { }

        public Rect2d(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public static Rect2d Zero = new Rect2d(0, 0, 0, 0);

        public float X;

        public float Y;

        public float Width;

        public float Height;

        public Position2d Position => new Position2d(this.X, this.Y);

        public Size2d Size => new Size2d(this.Width, this.Height);

        public override string ToString()
        {
            return $"[ {X}, {Y}, {Width}, {Height} ]";
        }
    }
}
