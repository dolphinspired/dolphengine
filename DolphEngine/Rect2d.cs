namespace DolphEngine
{
    public struct Rect2d
    {
        #region Constructors

        public Rect2d(Rect2d rect) 
            : this(rect.X, rect.Y, rect.Width, rect.Height) { }

        public Rect2d(Position2d position, Size2d size) 
            : this(position.X, position.Y, size.Width, size.Height) { }
        
        public Rect2d(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public static Rect2d Zero = new Rect2d(0, 0, 0, 0);

        #endregion

        #region Properties

        public float X;

        public float Y;

        public float Width;

        public float Height;

        #endregion

        #region Public methods

        public Position2d GetPosition()
        {
            return this.GetPosition(Anchor2d.TopLeft);
        }

        public Position2d GetPosition(Anchor2d anchor)
        {
            float x = X;
            float y = Y;

            if ((anchor & Anchor2d.Center) == Anchor2d.Center)
            {
                x += Width / 2;
            }
            else if ((anchor & Anchor2d.Right) == Anchor2d.Right)
            {
                x += Width;
            }

            if ((anchor & Anchor2d.Middle) == Anchor2d.Middle)
            {
                y += Height / 2;
            }
            else if ((anchor & Anchor2d.Bottom) == Anchor2d.Bottom)
            {
                y += Height;
            }

            return new Position2d(x, y);
        }

        public Rect2d SetPosition(float x, float y)
        {
            return this.SetPosition(x, y, Anchor2d.TopLeft);
        }

        public Rect2d SetPosition(float x, float y, Anchor2d anchor)
        {
            if ((anchor & Anchor2d.Center) == Anchor2d.Center)
            {
                x -= Width / 2;
            }
            else if ((anchor & Anchor2d.Right) == Anchor2d.Right)
            {
                x -= Width;
            }

            if ((anchor & Anchor2d.Middle) == Anchor2d.Middle)
            {
                y -= Height / 2;
            }
            else if ((anchor & Anchor2d.Bottom) == Anchor2d.Bottom)
            {
                y -= Height;
            }

            this.X = x;
            this.Y = y;
            return this;
        }

        public Size2d GetSize()
        {
            return new Size2d(this.Width, this.Height);
        }

        #endregion

        #region Object overrides

        public override string ToString()
        {
            return $"[ {X}, {Y}, {Width}, {Height} ]";
        }

        #endregion
    }
}
