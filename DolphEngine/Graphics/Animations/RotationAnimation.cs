using System;

namespace DolphEngine.Graphics.Animations
{
    public class RotationAnimation : KeyframeAnimation<Rotation2d>
    {
        public Origin2d Origin = Origin2d.TrueCenter;

        public RotationAnimation AddKeyframe(TimeSpan time, float radians)
        {
            base.AddKeyframe(time, new Rotation2d(radians));
            return this;
        }

        public RotationAnimation AddKeyframeDegrees(TimeSpan time, float degrees)
        {
            base.AddKeyframe(time, Rotation2d.FromDegrees(degrees));
            return this;
        }

        public new RotationAnimation AddKeyframe(TimeSpan time, Rotation2d keyframe)
        {
            base.AddKeyframe(time, keyframe);
            return this;
        }

        public new RotationAnimation Loop()
        {
            base.Loop();
            return this;
        }

        public new RotationAnimation Loop(TimeSpan time)
        {
            base.Loop(time);
            return this;
        }

        public RotationAnimation SetOrigin(Anchor2d anchor)
        {
            this.Origin = new Origin2d(anchor);
            return this;
        }

        public RotationAnimation SetOrigin(Origin2d origin)
        {
            this.Origin = origin;
            return this;
        }

        protected override Rotation2d Tween(Rotation2d prevKeyframe, Rotation2d nextKeyframe, double elapsedRatio)
        {
            // Perform a linear transition between the two rotations
            var radians = (float)(prevKeyframe.Radians + ((nextKeyframe.Radians - prevKeyframe.Radians) * elapsedRatio));
            return new Rotation2d(radians);
        }
    }
}
