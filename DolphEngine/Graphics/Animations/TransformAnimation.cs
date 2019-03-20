using System;

namespace DolphEngine.Graphics.Animations
{
    public class TransformAnimation : KeyframeAnimation<Transform2d>
    {
        public TransformAnimation(bool useDefaultZeroFrame = true)
        {
            if (useDefaultZeroFrame)
            {
                this.AddKeyframe(TimeSpan.Zero, Transform2d.None);
            }
        }

        public override Transform2d Tween(Transform2d prevKeyframe, Transform2d nextKeyframe, double elapsedRatio)
        {
            // Perform a simple linear transition between keyframes
            var xOffset = (float)(prevKeyframe.Offset.X + ((nextKeyframe.Offset.X - prevKeyframe.Offset.X) * elapsedRatio));
            var yOffset = (float)(prevKeyframe.Offset.Y + ((nextKeyframe.Offset.Y - prevKeyframe.Offset.Y) * elapsedRatio));
            var xScale = (float)(prevKeyframe.Scale.X + ((nextKeyframe.Scale.X - prevKeyframe.Scale.X) * elapsedRatio));
            var yScale = (float)(prevKeyframe.Scale.X + ((nextKeyframe.Scale.X - prevKeyframe.Scale.X) * elapsedRatio));
            var rotation = (float)(prevKeyframe.Rotation + ((nextKeyframe.Rotation - prevKeyframe.Rotation) * elapsedRatio));

            return new Transform2d(xOffset, yOffset, xScale, yScale, rotation);
        }
    }
}
