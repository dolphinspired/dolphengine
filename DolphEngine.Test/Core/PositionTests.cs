using Xunit;

namespace DolphEngine.Test.Core
{
    public class PositionTests
    {
        [Fact]
        public void CanMoveToPosition()
        {
            var pos = new Position2d(1, 2);
            pos.MoveTo(3, 4);

            DolphAssert.EqualF(3, pos.X);
            DolphAssert.EqualF(4, pos.Y);
        }

        [Theory]
        [InlineData( 1,  2,  3,  4,  4,  6)]
        [InlineData(-1, -2,  3,  4,  2,  2)]
        [InlineData( 1,  2, -3, -4, -2, -2)]
        [InlineData(-1, -2, -3, -4, -4, -6)]
        public void CanShiftPosition(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            var pos = new Position2d(x1, y1);
            pos.Shift(x2, y2);

            DolphAssert.EqualF(x3, pos.X);
            DolphAssert.EqualF(y3, pos.Y);
        }

        [Fact]
        public void CanConvertToVector()
        {
            var pos = new Position2d(1, 2);
            var vec = pos.ToVector();

            DolphAssert.EqualF(pos.X, vec.X);
            DolphAssert.EqualF(pos.Y, vec.Y);
        }

        [Theory]
        [InlineData( 0,  0)]
        [InlineData( 1,  2)]
        [InlineData(-1,  2)]
        [InlineData( 1, -2)]
        [InlineData(-1, -2)]
        public void TestEquality(float x, float y)
        {
            var pos1 = new Position2d(x, y);
            var pos2 = new Position2d(x, y);

            Assert.True(pos1 == pos2);
            Assert.True(pos1.Equals(pos2));

            pos1.X += Constants.FloatTolerance / 10;

            Assert.True(pos1 == pos2);
            Assert.True(pos1.Equals(pos2));

            pos1.X += Constants.FloatTolerance * 10;

            Assert.False(pos1 == pos2);
            Assert.False(pos1.Equals(pos2));
        }

        [Theory]
        [InlineData( 1,  2,  3,  4,  4,  6)]
        [InlineData(-1, -2,  3,  4,  2,  2)]
        [InlineData( 1,  2, -3, -4, -2, -2)]
        [InlineData(-1, -2, -3, -4, -4, -6)]
        public void TestAdd(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            var pos = new Position2d(x1, y1);
            pos += new Position2d(x2, y2);

            DolphAssert.EqualF(x3, pos.X);
            DolphAssert.EqualF(y3, pos.Y);

            pos = new Position2d(x1, y1);
            pos += new Vector2d(x2, y2);

            DolphAssert.EqualF(x3, pos.X);
            DolphAssert.EqualF(y3, pos.Y);
        }

        [Theory]
        [InlineData( 1,  2,  3,  4, -2, -2)]
        [InlineData(-1, -2,  3,  4, -4, -6)]
        [InlineData( 1,  2, -3, -4,  4,  6)]
        [InlineData(-1, -2, -3, -4,  2,  2)]
        public void TestSubtract(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            var pos = new Position2d(x1, y1);
            pos -= new Position2d(x2, y2);

            DolphAssert.EqualF(x3, pos.X);
            DolphAssert.EqualF(y3, pos.Y);

            pos = new Position2d(x1, y1);
            pos -= new Vector2d(x2, y2);

            DolphAssert.EqualF(x3, pos.X);
            DolphAssert.EqualF(y3, pos.Y);
        }

        [Theory]
        [InlineData( 0,  0,  0,  0)]
        [InlineData( 1,  1, -1, -1)]
        [InlineData( 1, -1, -1,  1)]
        [InlineData(-1,  1,  1, -1)]
        [InlineData(-1, -1,  1,  1)]
        public void TestNegative(float x1, float y1, float x2, float y2)
        {
            var pos = new Position2d(x1, y1);
            pos = -pos;

            DolphAssert.EqualF(x2, pos.X);
            DolphAssert.EqualF(y2, pos.Y);
        }

        [Fact]
        public void TestHashCode()
        {
            var pos1 = new Position2d(1, 2);
            var pos2 = new Position2d(1, 2);

            Assert.Equal(pos1.GetHashCode(), pos2.GetHashCode());

            pos2.Shift(1, 1);

            Assert.NotEqual(pos1.GetHashCode(), pos2.GetHashCode());
        }
    }
}
