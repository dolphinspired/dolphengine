namespace TacticsGame.Engine
{
    public struct ShortCoordinate2d
    {
        public ShortCoordinate2d(short x, short y)
        {
            this.X = x;
            this.Y = y;
        }

        public readonly short X;

        public readonly short Y;

        #region Operator overloads

        public static bool operator ==(ShortCoordinate2d o1, ShortCoordinate2d o2)
        {
            return o1.X == o2.X && o1.Y == o2.Y;
        }

        public static bool operator !=(ShortCoordinate2d o1, ShortCoordinate2d o2)
        {
            return !(o1 == o2);
        }

        #endregion

        #region Object overrides

        public override string ToString()
        {
            return $"({this.X},{this.Y})";
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ShortCoordinate2d))
            {
                return false;
            }

            return this == (ShortCoordinate2d)obj;
        }

        #endregion
    }
}
