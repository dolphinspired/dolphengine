using System;

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

        #region Operators

        public static bool operator ==(Size2d s1, Size2d s2)
        {
            return Math.Abs(s1.Width - s2.Width) < Constants.FloatTolerance && Math.Abs(s1.Height - s2.Height) < Constants.FloatTolerance;
        }

        public static bool operator !=(Size2d s1, Size2d s2)
        {
            return !(s1 == s2);
        }

        #endregion

        #region Object overrides

        public override bool Equals(object obj)
        {
            return (obj is Size2d) && this == (Size2d)obj;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 163;
                hash = hash * 167 + Width.GetHashCode();
                hash = hash * 167 + Height.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"[ {Width}, {Height} ]";
        }

        #endregion
    }
}
