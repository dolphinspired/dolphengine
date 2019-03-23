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

        public Position2d Set(float x, float y)
        {
            this.X = x;
            this.Y = y;
            return this;
        }

        #endregion

        #region Object overrides

        public override string ToString()
        {
            return $"[ {X}, {Y} ]";
        }

        #endregion
    }
}
