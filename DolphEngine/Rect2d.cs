namespace DolphEngine
{
    public struct Rect2d
    {
        public Rect2d(Rect2d rect) : this(rect.X, rect.Y, rect.Width, rect.Height) { }

        public Rect2d(Position2d position, Size2d size) : this(position.X, position.Y, size.Width, size.Height) { }

        public Rect2d(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public int X;

        public int Y;

        public int Width;

        public int Height;

        public Position2d Position => new Position2d(this.X, this.Y);

        public Size2d Size => new Size2d(this.Width, this.Height);

        public override string ToString()
        {
            return $"[ {X}, {Y}, {Width}, {Height} ]";
        }
    }
}
