namespace DolphEngine
{
    public struct Rect2d
    {
        #region Constructors

        private static readonly Origin2d DefaultOrigin = Origin2d.TopLeft;

        public Rect2d(Rect2d rect)
        {
            this.X = rect.X;
            this.Y = rect.Y;
            this.Width = rect.Width;
            this.Height = rect.Height;
            this.Origin = rect.Origin;
        }

        public Rect2d(Position2d position, Size2d size) : this(position, size, DefaultOrigin) { }

        public Rect2d(Position2d position, Size2d size, Anchor2d anchor) : this (position, size, new Origin2d(anchor)) { }

        public Rect2d(Position2d position, Size2d size, Origin2d origin)
        {
            this.X = position.X;
            this.Y = position.Y;
            this.Width = size.Width;
            this.Height = size.Height;
            this.Origin = origin;
        }

        public Rect2d(float x, float y, float width, float height) : this(x, y, width, height, DefaultOrigin) { }

        public Rect2d(float x, float y, float width, float height, Anchor2d anchor) : this(x, y, width, height, new Origin2d(anchor)) { }

        public Rect2d(float x, float y, float width, float height, Origin2d origin)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Origin = origin;
        }

        public static Rect2d Zero = new Rect2d(0, 0, 0, 0);

        #endregion

        #region Properties

        public float X;

        public float Y;

        public float Width;

        public float Height;

        public Origin2d Origin;

        #endregion

        #region Derived properties

        public Position2d TopLeft => this.GetAnchorPosition(Anchor2d.TopLeft);

        public Position2d TopRight => this.GetAnchorPosition(Anchor2d.TopRight);

        public Position2d BottomRight => this.GetAnchorPosition(Anchor2d.BottomRight);

        public Position2d BottomLeft => this.GetAnchorPosition(Anchor2d.BottomLeft);

        #endregion

        #region Public methods

        public Size2d GetSize()
        {
            return new Size2d(this.Width, this.Height);
        }

        public Position2d GetOriginPosition()
        {
            return this.GetAnchorPosition(this.Origin.Anchor);
        }

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

            float totalWidthDiff = (this.Width * widthDiff) + this.Origin.Offset.X;
            float totalHeightDiff = (this.Height * heightDiff) + this.Origin.Offset.Y;

            float x = this.X - totalWidthDiff;
            float y = this.Y - totalHeightDiff;

            return new Position2d(x, y);
        }

        public Polygon2d ToPolygon()
        {
            return new Polygon2d(TopLeft, TopRight, BottomRight, BottomLeft).Close();
        }

        public Rect2d MoveTo(Position2d position)
        {
            this.X = position.X;
            this.Y = position.Y;
            return this;
        }

        public Rect2d MoveTo(float x, float y)
        {
            this.X = x;
            this.Y = y;
            return this;
        }

        public Rect2d MoveTo(Rect2d rect)
        {
            this.X = rect.X;
            this.Y = rect.Y;
            return this;
        }

        public Rect2d Shift(Vector2d vector)
        {
            this.X += vector.X;
            this.Y += vector.Y;
            return this;
        }

        public Rect2d Shift(float x, float y)
        {
            this.X += x;
            this.Y += y;
            return this;
        }

        public Rect2d Scale(float magnitude)
        {
            this.Width *= magnitude;
            this.Height *= magnitude;
            return this;
        }

        public Rect2d Scale(float x, float y)
        {
            this.Width *= x;
            this.Height *= y;
            return this;
        }

        public Rect2d Scale(Vector2d scale)
        {
            this.Width *= scale.X;
            this.Height *= scale.Y;
            return this;
        }

        #endregion

        #region Operators

        public static bool operator ==(Rect2d r1, Rect2d r2)
        {
            return r1.X == r2.X && r1.Y == r2.Y && r1.Width == r2.Width && r1.Height == r2.Height && r1.Origin == r2.Origin;
        }

        public static bool operator !=(Rect2d r1, Rect2d r2)
        {
            return !(r1 == r2);
        }

        #endregion

        #region Object overrides

        public override bool Equals(object obj)
        {
            return (obj is Rect2d) && this == (Rect2d)obj;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 67;
                hash = hash * 71 + X.GetHashCode();
                hash = hash * 71 + Y.GetHashCode();
                hash = hash * 71 + Width.GetHashCode();
                hash = hash * 71 + Height.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"{{ pos: [ {X}, {Y} ], size: [ {Width}, {Height} ], origin: {Origin} }}";
        }

        #endregion
    }
}
