using System;

namespace DolphEngine
{
    public struct Position2d
    {
        #region Constructors

        public Position2d(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public static readonly Position2d Zero = new Position2d(0, 0);

        #endregion

        #region Properties

        public float X;

        public float Y;

        #endregion

        #region Public methods

        public void MoveTo(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public void Shift(float x, float y)
        {
            this.X += x;
            this.Y += y;
        }

        public Vector2d ToVector()
        {
            return new Vector2d(X, Y);
        }

        #endregion

        #region Operators

        public static Position2d operator -(Position2d v)
        {
            v.X = -v.X;
            v.Y = -v.Y;
            return v;
        }

        public static Position2d operator +(Position2d p1, Position2d p2)
        {
            p1.X += p2.X;
            p1.Y += p2.Y;
            return p1;
        }

        public static Position2d operator +(Position2d p, Vector2d v)
        {
            p.X += v.X;
            p.Y += v.Y;
            return p;
        }

        public static Position2d operator -(Position2d p1, Position2d p2)
        {
            p1.X -= p2.X;
            p1.Y -= p2.Y;
            return p1;
        }

        public static Position2d operator -(Position2d p, Vector2d v)
        {
            p.X -= v.X;
            p.Y -= v.Y;
            return p;
        }

        public static bool operator ==(Position2d p1, Position2d p2)
        {
            return Math.Abs(p1.X - p2.X) < Constants.FloatTolerance && Math.Abs(p1.Y - p2.Y) < Constants.FloatTolerance;
        }

        public static bool operator !=(Position2d p1, Position2d p2)
        {
            return !(p1 == p2);
        }

        #endregion

        #region Object overrides

        public override bool Equals(object obj)
        {
            return (obj is Position2d) && this == (Position2d)obj;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 151;
                hash = hash * 157 + X.GetHashCode();
                hash = hash * 157 + Y.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"[ {X}, {Y} ]";
        }

        #endregion
    }

    public abstract class Position2dBase : IPosition2d
    {
        public virtual float X
        {
            get => this._pos.X;
            set => this._pos.X = value;
        }

        public virtual float Y
        {
            get => this._pos.Y;
            set => this._pos.Y = value;
        }

        private Position2d _pos;
        public virtual Position2d Position
        {
            get => this._pos;
            set => this._pos = value;
        }

        public override string ToString()
        {
            return this._pos.ToString();
        }
    }

    public interface IPosition2d
    {
        float X { get; set; }

        float Y { get; set; }
    }

    public static class Position2dExtensions
    {
        #region Public methods

        public static IPosition2d MoveTo(this IPosition2d pos, float x, float y)
        {
            pos.X = x;
            pos.Y = y;
            return pos;
        }

        public static IPosition2d MoveTo(this IPosition2d pos, Position2d position)
        {
            return pos.MoveTo(position.X, position.Y);
        }

        public static IPosition2d Shift(this IPosition2d pos, float x, float y)
        {
            pos.X += x;
            pos.Y += y;
            return pos;
        }

        public static IPosition2d Shift(this IPosition2d pos, Vector2d vector)
        {
            return pos.Shift(vector.X, vector.Y);
        }

        #endregion
    }
}
