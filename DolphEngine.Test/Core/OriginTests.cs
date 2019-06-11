using Xunit;

namespace DolphEngine.Test.Core
{
    public class OriginTests
    {
        [Theory]
        [InlineData(Anchor2d.TopLeft, 0, 0)]
        [InlineData(Anchor2d.Center, 0.5, 6.8)]
        [InlineData(Anchor2d.BottomRight, -4, -12)]
        public void TestEquality(Anchor2d anchor, float x, float y)
        {
            var o1 = new Origin2d(anchor, new Vector2d(x, y));
            var o2 = new Origin2d(anchor, new Vector2d(x, y));

            Assert.True(o1 == o2);
            Assert.True(o1.Equals(o2));

            o1.Offset.X += Constants.FloatTolerance / 10;

            Assert.True(o1 == o2);
            Assert.True(o1.Equals(o2));

            o1.Offset.X += Constants.FloatTolerance * 10;

            Assert.False(o1 == o2);
            Assert.False(o1.Equals(o2));
        }

        [Fact]
        public void TestHashCode()
        {
            var o1 = new Origin2d(Anchor2d.Center, new Vector2d(8, 9));
            var o2 = new Origin2d(Anchor2d.Center, new Vector2d(8, 9));

            Assert.Equal(o1.GetHashCode(), o2.GetHashCode());

            o2.Offset.X += 1;

            Assert.NotEqual(o1.GetHashCode(), o2.GetHashCode());
        }
    }
}
