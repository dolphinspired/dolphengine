namespace DolphEngine
{
    public struct Size2d
    {
        #region Constructors

        public Size2d(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }

        public static readonly Size2d Zero = new Size2d(0, 0);

        #endregion

        #region Properties

        public float Width;

        public float Height;

        #endregion

        #region Public methods

        public Size2d Scale(float magnitude)
        {
            this.Width *= magnitude;
            this.Height *= magnitude;
            return this;
        }

        public Size2d Scale(float x, float y)
        {
            this.Width *= x;
            this.Height *= y;
            return this;
        }

        public Size2d Scale(Vector2d scale)
        {
            this.Width *= scale.X;
            this.Height *= scale.Y;
            return this;
        }

        #endregion

        #region Object overrides

        public override string ToString()
        {
            return $"[ {Width}, {Height} ]";
        }

        #endregion
    }
}
