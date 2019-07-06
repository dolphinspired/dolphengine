using System;
using System.Collections.Generic;
using Xunit;

namespace DolphEngine.Test.Core
{
    public class RectTests
    {
        #region Constructor tests

        [Fact]
        public void CanCreateFromRect()
        {
            var rect1 = new Rect2d(1, 2, 3, 4);
            var rect2 = new Rect2d(rect1);
            DolphAssert.EqualF(rect1.X, rect2.X);
            DolphAssert.EqualF(rect1.Y, rect2.Y);
            DolphAssert.EqualF(rect1.Width, rect2.Width);
            DolphAssert.EqualF(rect1.Height, rect2.Height);
            Assert.Equal(rect1.Origin.Anchor, rect2.Origin.Anchor);
        }

        [Fact]
        public void CanCreateFromPositionAndSize()
        {
            var pos = new Position2d(1, 2);
            var size = new Size2d(3, 4);
            var anchor = Anchor2d.Center;

            var rect = new Rect2d(pos, size);
            DolphAssert.EqualF(pos.X, rect.X);
            DolphAssert.EqualF(pos.Y, rect.Y);
            DolphAssert.EqualF(size.Width, rect.Width);
            DolphAssert.EqualF(size.Height, rect.Height);

            rect = new Rect2d(pos, size, anchor);
            DolphAssert.EqualF(pos.X, rect.X);
            DolphAssert.EqualF(pos.Y, rect.Y);
            DolphAssert.EqualF(size.Width, rect.Width);
            DolphAssert.EqualF(size.Height, rect.Height);
            Assert.Equal(anchor, rect.Origin.Anchor);

            rect = new Rect2d(pos, size, new Origin2d(anchor));
            DolphAssert.EqualF(pos.X, rect.X);
            DolphAssert.EqualF(pos.Y, rect.Y);
            DolphAssert.EqualF(size.Width, rect.Width);
            DolphAssert.EqualF(size.Height, rect.Height);
            Assert.Equal(anchor, rect.Origin.Anchor);
        }

        [Fact]
        public void CanCreateFromValues()
        {
            var x = 1;
            var y = 2;
            var width = 3;
            var height = 4;
            var anchor = Anchor2d.MiddleRight;

            var rect = new Rect2d(x, y, width, height);
            DolphAssert.EqualF(x, rect.X);
            DolphAssert.EqualF(y, rect.Y);
            DolphAssert.EqualF(width, rect.Width);
            DolphAssert.EqualF(height, rect.Height);

            rect = new Rect2d(x, y, width, height, anchor);
            DolphAssert.EqualF(x, rect.X);
            DolphAssert.EqualF(y, rect.Y);
            DolphAssert.EqualF(width, rect.Width);
            DolphAssert.EqualF(height, rect.Height);
            Assert.Equal(anchor, rect.Origin.Anchor);

            rect = new Rect2d(x, y, width, height, new Origin2d(anchor));
            DolphAssert.EqualF(x, rect.X);
            DolphAssert.EqualF(y, rect.Y);
            DolphAssert.EqualF(width, rect.Width);
            DolphAssert.EqualF(height, rect.Height);
            Assert.Equal(anchor, rect.Origin.Anchor);
        }

        #endregion

        #region Anchor position tests

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

        #endregion

        #region Transformation tests

        [Fact]
        public void CanMoveToPosition()
        {
            var width = 7;
            var height = 8;

            var rect = new Rect2d(1, 2, width, height);
            rect.MoveTo(3, 4);

            DolphAssert.EqualF(3, rect.X);
            DolphAssert.EqualF(4, rect.Y);
            DolphAssert.EqualF(width, rect.Width);
            DolphAssert.EqualF(height, rect.Height);
        }

        [Theory]
        [InlineData( 1,  2,  3,  4,  4,  6)]
        [InlineData(-1, -2,  3,  4,  2,  2)]
        [InlineData( 1,  2, -3, -4, -2, -2)]
        [InlineData(-1, -2, -3, -4, -4, -6)]
        public void CanShiftPosition(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            var width = 30;
            var height = 40;

            var rect = new Rect2d(x1, y1, width, height, Anchor2d.BottomLeft);
            rect.Shift(x2, y2);

            DolphAssert.EqualF(x3, rect.X);
            DolphAssert.EqualF(y3, rect.Y);
            DolphAssert.EqualF(width, rect.Width);
            DolphAssert.EqualF(height, rect.Height);
        }

        [Theory]
        [InlineData(  0,   0, 100,   0,   0)]
        [InlineData( 10,  20, 1.5,  15,  30)]
        [InlineData(-10,  20, 1.5, -15,  30)]
        [InlineData(-10, -20, 1.5, -15, -30)]
        [InlineData( 10,  20, 0.5,   5,  10)]
        [InlineData(-10,  20, 0.5,  -5,  10)]
        [InlineData(-10, -20, 0.5,  -5, -10)]
        [InlineData( 10,  20,  -2, -20, -40)]
        [InlineData(-10,  20,  -2,  20, -40)]
        [InlineData(-10, -20,  -2,  20,  40)]
        public void CanScaleByMagnitude(float w1, float h1, float mag, float w2, float h2)
        {
            var x = 15;
            var y = 25;

            var rect = new Rect2d(x, y, w1, h1);
            rect.Scale(mag, mag);

            DolphAssert.EqualF(x, rect.X);
            DolphAssert.EqualF(y, rect.Y);
            DolphAssert.EqualF(w2, rect.Width);
            DolphAssert.EqualF(h2, rect.Height);
        }

        #endregion

        [Fact]
        public void CanConvertToSize()
        {
            var size1 = new Size2d(8, 12);
            var rect = new Rect2d(new Position2d(4, 4), size1);
            var size2 = rect.GetSize();

            Assert.Equal(size1, size2);
        }

        [Fact]
        public void CanGetPositionAtOrigin()
        {
            var anchor = Anchor2d.MiddleRight;
            var rect = new Rect2d(100, 200, 300, 400, anchor);
            var pos1 = rect.GetAnchorPosition(anchor);
            var pos2 = rect.GetOriginPosition();

            Assert.Equal(pos1, pos2);
        }

        [Fact]
        public void CanConvertToPolygon()
        {
            var rect = new Rect2d(100, 200, 300, 400, Anchor2d.BottomCenter);
            var poly = rect.ToPolygon();

            // Extra point from where corner 4 closes back to corner 1
            Assert.Equal(5, poly.Points.Count);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, Anchor2d.TopLeft)]
        [InlineData(1, 2, -3, -4, Anchor2d.BottomLeft)]
        [InlineData(-1, 2, 0.5, -0.25, Anchor2d.TopRight)]
        [InlineData(1, -2.5, -3, 40.44, Anchor2d.BottomRight)]
        [InlineData(-1, -2, 8, 2100, Anchor2d.Center)]
        public void TestEquality(float x, float y, float w, float h, Anchor2d anchor)
        {
            var rect1 = new Rect2d(x, y, w, h, anchor);
            var rect2 = new Rect2d(x, y, w, h, anchor);

            Assert.True(rect1 == rect2);
            Assert.True(rect1.Equals(rect2));

            rect1.X += Constants.FloatTolerance / 10;

            Assert.True(rect1 == rect2);
            Assert.True(rect1.Equals(rect2));

            rect1.X += Constants.FloatTolerance * 10;

            Assert.False(rect1 == rect2);
            Assert.False(rect1.Equals(rect2));
        }

        [Fact]
        public void TestHashCode()
        {
            var rect1 = new Rect2d(1, 2, 3, 4);
            var rect2 = new Rect2d(1, 2, 3, 4);

            Assert.Equal(rect1.GetHashCode(), rect2.GetHashCode());

            rect2.Shift(1, 1);

            Assert.NotEqual(rect1.GetHashCode(), rect2.GetHashCode());
        }
    }
}
