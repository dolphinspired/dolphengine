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

        #endregion

        #region Object overrides

        public override string ToString()
        {
            return $"[ {X}, {Y} ]";
        }

        #endregion
    }
}
