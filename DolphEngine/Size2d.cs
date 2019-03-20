namespace DolphEngine
{
    public struct Size2d
    {
        public static Size2d Zero = new Size2d(0, 0);

        public Size2d(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }

        public float Width;

        public float Height;

        public override string ToString()
        {
            return $"[ {Width}, {Height} ]";
        }
    }
}
