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

        public void Scale(float x, float y)
        {
            this.Width *= x;
            this.Height *= y;
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

    public abstract class Size2dBase : ISize2d
    {
        public virtual float Width
        {
            get => this._size.Width;
            set => this._size.Width = value;
        }

        public virtual float Height
        {
            get => this._size.Height;
            set => this._size.Height = value;
        }

        private Size2d _size;
        public virtual Size2d Size
        {
            get => this._size;
            set => this._size = value;
        }

        public override string ToString()
        {
            return this._size.ToString();
        }
    }

    public interface ISize2d
    {
        float Width { get; set; }

        float Height { get; set; }
    }

    public static class Size2dExtensions
    {
        public static ISize2d Scale(this ISize2d size, float x, float y)
        {
            size.Width *= x;
            size.Height *= y;
            return size;
        }

        public static ISize2d Scale(this ISize2d size, float magnitude)
        {
            return size.Scale(magnitude, magnitude);
        }

        public static ISize2d Scale(this ISize2d size, Vector2d scale)
        {
            return size.Scale(scale.X, scale.Y);
        }
    }
}
