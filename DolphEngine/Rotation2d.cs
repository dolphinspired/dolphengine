using System;

namespace DolphEngine
{
    public struct Rotation2d
    {
        private const float R2D = 180 / MathF.PI;
        private const float D2R = MathF.PI / 180;

        #region Constructors

        public Rotation2d(float radians)
        {
            this.Radians = radians;
        }

        public static readonly Rotation2d Zero = new Rotation2d(0);

        public static Rotation2d FromDegrees(float degrees) => new Rotation2d(degrees * D2R);

        #endregion

        #region Properties

        public float Radians;

        public float Degrees => this.Radians * R2D;

        #endregion

        #region Public methods

        public Rotation2d Turn(float radians)
        {
            this.Radians += radians;
            return this;
        }

        public Rotation2d TurnDegrees(float degrees)
        {
            this.Radians += degrees * D2R;
            return this;
        }

        public Rotation2d Turn(Rotation2d rotation)
        {
            this.Radians += rotation.Radians;
            return this;
        }

        #endregion

        #region Operators

        public static bool operator ==(Rotation2d r1, Rotation2d r2)
        {
            return Math.Abs(r1.Radians - r2.Radians) < Constants.FloatTolerance;
        }

        public static bool operator !=(Rotation2d r1, Rotation2d r2)
        {
            return !(r1 == r2);
        }

        #endregion

        #region Object overrides

        public override bool Equals(object obj)
        {
            return (obj is Rotation2d) && this == (Rotation2d)obj;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 31;
                hash = hash * 37 + Radians.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"{{ rad: {Radians}, deg: {Degrees} }}";
        }

        #endregion
    }
}
