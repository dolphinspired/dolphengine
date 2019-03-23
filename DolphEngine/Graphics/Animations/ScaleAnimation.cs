using System;

namespace DolphEngine.Graphics.Animations
{
    public class ScaleAnimation : KeyframeAnimation<Vector2d>
    {
        public Origin2d Origin = Origin2d.TrueCenter;

        public ScaleAnimation AddKeyframe(TimeSpan time, float scale)
        {
            base.AddKeyframe(time, new Vector2d(scale, scale));
            return this;
        }

        public ScaleAnimation AddKeyframe(TimeSpan time, float xScale, float yScale)
        {
            base.AddKeyframe(time, new Vector2d(xScale, yScale));
            return this;
        }

        public new ScaleAnimation AddKeyframe(TimeSpan time, Vector2d keyframe)
        {
            base.AddKeyframe(time, keyframe);
            return this;
        }

        public new ScaleAnimation Loop()
        {
            base.Loop();
            return this;
        }

        public new ScaleAnimation Loop(TimeSpan time)
        {
            base.Loop(time);
            return this;
        }

        public ScaleAnimation SetOrigin(Anchor2d anchor)
        {
            this.Origin = new Origin2d(anchor);
            return this;
        }

        public ScaleAnimation SetOrigin(Origin2d origin)
        {
            this.Origin = origin;
            return this;
        }

        protected override Vector2d Tween(Vector2d prevKeyframe, Vector2d nextKeyframe, double elapsedRatio)
        {
            // Perform a linear transition between the two multipliers
            var xScale = (float)(prevKeyframe.X + ((nextKeyframe.X - prevKeyframe.X) * elapsedRatio));
            var yScale = (float)(prevKeyframe.X + ((nextKeyframe.X - prevKeyframe.X) * elapsedRatio));
            return new Vector2d(xScale, yScale);
        }
    }
}
