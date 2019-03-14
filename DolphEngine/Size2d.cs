namespace DolphEngine
{
    public struct Size2d
    {
        public static Size2d Zero = new Size2d(0, 0);

        public Size2d(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public int Width;

        public int Height;

        public override string ToString()
        {
            return $"[ {Width}, {Height} ]";
        }
    }
}
