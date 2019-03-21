using System;

namespace DolphEngine
{
    public struct Rotation2d
    {
        private const float R2D = 180 / MathF.PI;
        private const float D2R = MathF.PI / 180;

        #region Constructors

        public Rotation2d(float radians)
            : this(radians, Vector2d.Zero)
        {
        }

        public Rotation2d(float radians, float x, float y)
            : this(radians, new Vector2d(x, y))
        {
        }

        public Rotation2d(float radians, Vector2d origin)
        {
            this.Radians = radians;
            this.Origin = origin;
        }

        public Rotation2d(float radians, Rect2d area)
            : this(radians, area, Anchor2d.MiddleCenter)
        {
        }

        public Rotation2d(float radians, Rect2d area, Anchor2d anchor)
        {
            var pos = area.GetPosition(anchor);
            this.Radians = radians;
            this.Origin = new Vector2d(pos.X, pos.Y);
        }

        #endregion

        #region Static methods

        public static Rotation2d FromDegrees(float degrees) => new Rotation2d(degrees * D2R);

        public static Rotation2d FromDegrees(float degrees, float x, float y) => new Rotation2d(degrees * D2R, x, y);

        public static Rotation2d FromDegrees(float degrees, Vector2d origin) => new Rotation2d(degrees * D2R, origin);

        public static Rotation2d FromDegrees(float degrees, Rect2d area) => new Rotation2d(degrees * D2R, area);

        public static Rotation2d FromDegrees(float degrees, Rect2d area, Anchor2d anchor) => new Rotation2d(degrees * D2R, area, anchor);

        #endregion

        #region Properties

        public float Radians;

        public float Degrees => this.Radians * R2D;

        public Vector2d Origin;

        #endregion
    }
}
