using Xunit;

namespace DolphEngine.Test.Core
{
    public class RectTests
    {
        private const float X = 100;
        private const float Y = 200;
        private const float W = 30;
        private const float H = 50;

        [Theory]
        [InlineData(Anchor2d.Default,        X, Y)]
        [InlineData(Anchor2d.Left,           X, Y)]
        [InlineData(Anchor2d.Right,          X+W, Y)]
        [InlineData(Anchor2d.Center,         X+W/2, Y)]
        [InlineData(Anchor2d.Top,            X, Y)]
        [InlineData(Anchor2d.TopLeft,        X, Y)]
        [InlineData(Anchor2d.TopRight,       X+W, Y)]
        [InlineData(Anchor2d.TopCenter,      X+W/2, Y)]
        [InlineData(Anchor2d.Bottom,         X, Y+H)]
        [InlineData(Anchor2d.BottomLeft,     X, Y+H)]
        [InlineData(Anchor2d.BottomRight,    X+W, Y+H)]
        [InlineData(Anchor2d.BottomCenter,   X+W/2, Y+H)]
        [InlineData(Anchor2d.Middle,         X, Y+H/2)]
        [InlineData(Anchor2d.MiddleLeft,     X, Y+H/2)]
        [InlineData(Anchor2d.MiddleRight,    X+W, Y+H/2)]
        [InlineData(Anchor2d.MiddleCenter,   X+W/2, Y+H/2)]
        public void CanGetPositionWithAnchor(Anchor2d anchor, float expectedX, float expectedY)
        {
            var rect = new Rect2d(X, Y, W, H);
            var pos = rect.GetAnchorPosition(anchor);

            DolphAssert.EqualF(expectedX, pos.X);
            DolphAssert.EqualF(expectedY, pos.Y);
        }

        [Theory]
        [InlineData(Anchor2d.Default,        X, Y)]
        [InlineData(Anchor2d.Left,           X, Y)]
        [InlineData(Anchor2d.Right,          X-W, Y)]
        [InlineData(Anchor2d.Center,         X-W/2, Y)]
        [InlineData(Anchor2d.Top,            X, Y)]
        [InlineData(Anchor2d.TopLeft,        X, Y)]
        [InlineData(Anchor2d.TopRight,       X-W, Y)]
        [InlineData(Anchor2d.TopCenter,      X-W/2, Y)]
        [InlineData(Anchor2d.Bottom,         X, Y-H)]
        [InlineData(Anchor2d.BottomLeft,     X, Y-H)]
        [InlineData(Anchor2d.BottomRight,    X-W, Y-H)]
        [InlineData(Anchor2d.BottomCenter,   X-W/2, Y-H)]
        [InlineData(Anchor2d.Middle,         X, Y-H/2)]
        [InlineData(Anchor2d.MiddleLeft,     X, Y-H/2)]
        [InlineData(Anchor2d.MiddleRight,    X-W, Y-H/2)]
        [InlineData(Anchor2d.MiddleCenter,   X-W/2, Y-H/2)]
        public void CanSetPositionWithAnchor(Anchor2d anchor, float expectedX, float expectedY)
        {
            var rect = new Rect2d(X, Y, W, H, anchor);
            var pos = rect.GetAnchorPosition(Anchor2d.TopLeft);

            DolphAssert.EqualF(expectedX, pos.X);
            DolphAssert.EqualF(expectedY, pos.Y);
        }
    }
}
