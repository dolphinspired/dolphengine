namespace DolphEngine
{
    public struct Rect2d
    {
        #region Constructors

        private static readonly Origin2d DefaultOrigin = Origin2d.TopLeft;

        public Rect2d(Rect2d rect)
        {
            this.Position = rect.Position;
            this.Size = rect.Size;
            this.Origin = rect.Origin;
        }

        public Rect2d(Position2d position, Size2d size)
        {
            this.Position = position;
            this.Size = size;
            this.Origin = DefaultOrigin;
        }

        public Rect2d(Position2d position, Size2d size, Anchor2d anchor)
        {
            this.Position = position;
            this.Size = size;
            this.Origin = new Origin2d(anchor);
        }

        public Rect2d(Position2d position, Size2d size, Origin2d origin)
        {
            this.Position = position;
            this.Size = size;
            this.Origin = origin;
        }
        
        public Rect2d(float x, float y, float width, float height)
        {
            this.Position = new Position2d(x, y);
            this.Size = new Size2d(width, height);
            this.Origin = DefaultOrigin;
        }

        public Rect2d(float x, float y, float width, float height, Anchor2d anchor)
        {
            this.Position = new Position2d(x, y);
            this.Size = new Size2d(width, height);
            this.Origin = new Origin2d(anchor);
        }

        public Rect2d(float x, float y, float width, float height, Origin2d origin)
        {
            this.Position = new Position2d(x, y);
            this.Size = new Size2d(width, height);
            this.Origin = origin;
        }

        public static Rect2d Zero = new Rect2d(0, 0, 0, 0);

        #endregion

        #region Properties

        public Position2d Position;

        public Size2d Size;

        public Origin2d Origin;

        #endregion

        #region Derived properties

        public Position2d TopLeft => this.GetAnchorPosition(Anchor2d.TopLeft);

        public Position2d TopRight => this.GetAnchorPosition(Anchor2d.TopRight);

        public Position2d BottomRight => this.GetAnchorPosition(Anchor2d.BottomRight);

        public Position2d BottomLeft => this.GetAnchorPosition(Anchor2d.BottomLeft);

        #endregion

        #region Public methods

        public Position2d GetAnchorPosition(Anchor2d anchor)
        {
            const float None = 0.000f;
            const float Half = 0.500f;
            const float Whole = 1.000f;

            Anchor2d thisAnchor = this.Origin.Anchor;
            float widthDiff;
            float heightDiff;

            if ((anchor & Anchor2d.Center) == Anchor2d.Center)
            {
                if ((thisAnchor & Anchor2d.Center) == Anchor2d.Center)
                {
                    widthDiff = None;
                }
                else if ((thisAnchor & Anchor2d.Right) == Anchor2d.Right)
                {
                    widthDiff = Half;
                }
                else
                {
                    widthDiff = -Half;
                }
            }
            else if ((anchor & Anchor2d.Right) == Anchor2d.Right)
            {
                if ((thisAnchor & Anchor2d.Center) == Anchor2d.Center)
                {
                    widthDiff = -Half;
                }
                else if ((thisAnchor & Anchor2d.Right) == Anchor2d.Right)
                {
                    widthDiff = None;
                }
                else
                {
                    widthDiff = -Whole;
                }
            }
            else
            {
                if ((thisAnchor & Anchor2d.Center) == Anchor2d.Center)
                {
                    widthDiff = Half;
                }
                else if ((thisAnchor & Anchor2d.Right) == Anchor2d.Right)
                {
                    widthDiff = Whole;
                }
                else
                {
                    widthDiff = None;
                }
            }

            if ((anchor & Anchor2d.Middle) == Anchor2d.Middle)
            {
                if ((thisAnchor & Anchor2d.Middle) == Anchor2d.Middle)
                {
                    heightDiff = None;
                }
                else if ((thisAnchor & Anchor2d.Bottom) == Anchor2d.Bottom)
                {
                    heightDiff = Half;
                }
                else
                {
                    heightDiff = -Half;
                }
            }
            else if ((anchor & Anchor2d.Bottom) == Anchor2d.Bottom)
            {
                if ((thisAnchor & Anchor2d.Middle) == Anchor2d.Middle)
                {
                    heightDiff = -Half;
                }
                else if ((thisAnchor & Anchor2d.Bottom) == Anchor2d.Bottom)
                {
                    heightDiff = None;
                }
                else
                {
                    heightDiff = -Whole;
                }
            }
            else
            {
                if ((thisAnchor & Anchor2d.Middle) == Anchor2d.Middle)
                {
                    heightDiff = Half;
                }
                else if ((thisAnchor & Anchor2d.Bottom) == Anchor2d.Bottom)
                {
                    heightDiff = Whole;
                }
                else
                {
                    heightDiff = None;
                }
            }

            float totalWidthDiff = (this.Size.Width * widthDiff) + this.Origin.Offset.X;
            float totalHeightDiff = (this.Size.Height * heightDiff) + this.Origin.Offset.Y;

            float x = this.Position.X - totalWidthDiff;
            float y = this.Position.Y - totalHeightDiff;

            return new Position2d(x, y);
        }

        public Polygon2d ToPolygon()
        {
            return new Polygon2d(TopLeft, TopRight, BottomRight, BottomLeft).Close();
        }

        #endregion

        #region Object overrides

        public override string ToString()
        {
            return $"{{ pos: {Position}, size: {Size}, origin: {Origin} }}";
        }

        #endregion
    }
}
