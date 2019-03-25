using System;
using System.Collections.Generic;
using Xunit;

namespace DolphEngine.Test.Core
{
    public class RectTests
    {
        private const float X = 100;
        private const float Y = 200;
        private const float W = 30;
        private const float H = 50;

        // A mapping for how to get from top-left to any other anchor
        private static readonly Dictionary<Anchor2d, RelativePos> RpMap = new Dictionary<Anchor2d, RelativePos>
        {
            { Anchor2d.Default,         new RelativePos(x => x, y => y) },
            { Anchor2d.Left,            new RelativePos(x => x, y => y) },
            { Anchor2d.Right,           new RelativePos(x => x+W, y => y) },
            { Anchor2d.Center,          new RelativePos(x => x+W/2, y => y) },
            { Anchor2d.Top,             new RelativePos(x => x, y => y) },
            { Anchor2d.TopLeft,         new RelativePos(x => x, y => y) },
            { Anchor2d.TopRight,        new RelativePos(x => x+W, y => y) },
            { Anchor2d.TopCenter,       new RelativePos(x => x+W/2, y => y) },
            { Anchor2d.Bottom,          new RelativePos(x => x, y => y+H) },
            { Anchor2d.BottomLeft,      new RelativePos(x => x, y => y+H) },
            { Anchor2d.BottomRight,     new RelativePos(x => x+W, y => y+H) },
            { Anchor2d.BottomCenter,    new RelativePos(x => x+W/2, y => y+H) },
            { Anchor2d.Middle,          new RelativePos(x => x, y => y+H/2) },
            { Anchor2d.MiddleLeft,      new RelativePos(x => x, y => y+H/2) },
            { Anchor2d.MiddleRight,     new RelativePos(x => x+W, y => y+H/2) },
            { Anchor2d.MiddleCenter,    new RelativePos(x => x+W/2, y => y+H/2) },
        };

        private class RelativePos
        {
            public RelativePos(Func<float, float> xFunc, Func<float, float> yFunc)
            {
                this.X = xFunc;
                this.Y = yFunc;
            }

            public readonly Func<float, float> X;

            public readonly Func<float, float> Y;
        }

        // This essentially tests that the above mappings are correct
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
        public void CanSetPositionWithAnchor(Anchor2d anchor, float expectedTopLeftX, float expectedTopLeftY)
        {
            var rect = new Rect2d(X, Y, W, H, anchor);

            foreach (var kvp in RpMap)
            {
                var getAnchor = kvp.Key;
                var rp = kvp.Value;

                var pos = rect.GetAnchorPosition(getAnchor);
                DolphAssert.EqualF(rp.X(expectedTopLeftX), pos.X);
                DolphAssert.EqualF(rp.Y(expectedTopLeftY), pos.Y);
            }
        }
    }
}
