using System;

namespace DolphEngine
{
    public struct Vector2d
    {
        #region Constructors

        public Vector2d(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public static readonly Vector2d Zero = new Vector2d(0, 0);

        public static readonly Vector2d One = new Vector2d(1.000f, 1.000f);

        #endregion

        #region Properties

        public float X;

        public float Y;

        #endregion

        #region Public methods

        public Vector2d Set(float magnitude)
        {
            this.X = magnitude;
            this.Y = magnitude;
            return this;
        }

        public Vector2d Set(float x, float y)
        {
            this.X = x;
            this.Y = y;
            return this;
        }

        public Position2d ToPosition()
        {
            return new Position2d(X, Y);
        }

        #endregion

        #region Operators

        public static Vector2d operator -(Vector2d v)
        {
            v.X = -v.X;
            v.Y = -v.Y;
            return v;
        }

        public static Vector2d operator +(Vector2d v1, Vector2d v2)
        {
            v1.X += v2.X;
            v1.Y += v2.Y;
            return v1;
        }

        public static Vector2d operator -(Vector2d v1, Vector2d v2)
        {
            v1.X -= v2.X;
            v1.Y -= v2.Y;
            return v1;
        }

        public static Vector2d operator *(Vector2d v1, Vector2d v2)
        {
            v1.X *= v2.X;
            v1.Y *= v2.Y;
            return v1;
        }

        public static Vector2d operator /(Vector2d v1, Vector2d v2)
        {
            v1.X /= v2.X;
            v1.Y /= v2.Y;
            return v1;
        }

        public static bool operator ==(Vector2d v1, Vector2d v2)
        {
            return Math.Abs(v1.X - v2.X) < Constants.FloatTolerance && Math.Abs(v1.Y - v2.Y) < Constants.FloatTolerance;
        }

        public static bool operator !=(Vector2d v1, Vector2d v2)
        {
            return !(v1 == v2);
        }

        #endregion

        #region Object overrides

        public override bool Equals(object obj)
        {
            return (obj is Vector2d) && this == (Vector2d)obj;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 173;
                hash = hash * 179 + X.GetHashCode();
                hash = hash * 179 + Y.GetHashCode();
                return hash;
            }            
        }

        public override string ToString()
        {
            return $"[ {X}, {Y} ]";
        }

        #endregion
    }
}
