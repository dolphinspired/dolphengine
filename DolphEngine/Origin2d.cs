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

        public Anchor2d Anchor;

        public Vector2d Offset;

        #region Object overrides

        public override string ToString()
        {
            return $"{{ anchor: \"{Anchor}\", offset: {Offset} }}";
        }

        #endregion
    }
}
