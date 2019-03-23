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

        #region Object overrides

        public override string ToString()
        {
            return $"{{ rad: {Radians}, deg: {Degrees} }}";
        }

        #endregion
    }
}
