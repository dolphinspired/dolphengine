using System;

namespace DolphEngine.Graphics.Animations
{
    public class PositionAnimation : KeyframeAnimation<Vector2d>
    {
        public PositionAnimation AddKeyframe(TimeSpan time, float x, float y)
        {
            base.AddKeyframe(time, new Vector2d(x, y));
            return this;
        }

        public new PositionAnimation AddKeyframe(TimeSpan time, Vector2d keyframe)
        {
            base.AddKeyframe(time, keyframe);
            return this;
        }

        public new PositionAnimation Loop()
        {
            base.Loop();
            return this;
        }

        public new PositionAnimation Loop(TimeSpan time)
        {
            base.Loop(time);
            return this;
        }

        protected override Vector2d Tween(Vector2d prevKeyframe, Vector2d nextKeyframe, double elapsedRatio)
        {
            // Perform a linear transition between the two points
            var xOffset = (float)(prevKeyframe.X + ((nextKeyframe.X - prevKeyframe.X) * elapsedRatio));
            var yOffset = (float)(prevKeyframe.Y + ((nextKeyframe.Y - prevKeyframe.Y) * elapsedRatio));
            return new Vector2d(xOffset, yOffset);
        }
    }
}
