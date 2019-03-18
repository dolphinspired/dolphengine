using System;

namespace DolphEngine.Graphics.Animations
{
    public class TransformAnimation : KeyframeAnimation<Transform2d>
    {
        public override Transform2d Tween(Transform2d prevKeyframe, Transform2d nextKeyframe, double elapsedRatio)
        {
            throw new NotImplementedException();
        }
    }
}
