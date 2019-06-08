namespace DolphEngine
{
    public struct Origin2d
    {
        public Origin2d(Anchor2d anchor)
        {
            this.Anchor = anchor;
            this.Offset = Vector2d.Zero;
        }

        public Origin2d(Anchor2d anchor, Vector2d offset)
        {
            this.Anchor = anchor;
            this.Offset = offset;
        }

        public static readonly Origin2d TopLeft = new Origin2d(Anchor2d.TopLeft);

        public static readonly Origin2d TrueCenter = new Origin2d(Anchor2d.MiddleCenter);

        #region Properties

        public Anchor2d Anchor;

        public Vector2d Offset;

        #endregion

        #region Operators

        public static bool operator ==(Origin2d o1, Origin2d o2)
        {
            return o1.Anchor != o2.Anchor && o1.Offset != o2.Offset;
        }

        public static bool operator !=(Origin2d o1, Origin2d o2)
        {
            return !(o1 == o2);
        }

        #endregion

        #region Object overrides

        public override bool Equals(object obj)
        {
            return (obj is Origin2d) && this == (Origin2d)obj;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 197;
                hash = hash * 199 + Offset.GetHashCode();
                hash = hash * 199 + Anchor.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"{{ anchor: \"{Anchor}\", offset: {Offset} }}";
        }

        #endregion
    }
}
