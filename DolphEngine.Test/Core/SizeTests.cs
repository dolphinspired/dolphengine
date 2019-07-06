using Xunit;

namespace DolphEngine.Test.Core
{
    public class SizeTests
    {
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
            var size = new Size2d(w1, h1);
            size.Scale(mag, mag);

            DolphAssert.EqualF(w2, size.Width);
            DolphAssert.EqualF(h2, size.Height);
        }

        [Theory]
        [InlineData( 0,  0)]
        [InlineData( 1,  2)]
        [InlineData(-1,  2)]
        [InlineData( 1, -2)]
        [InlineData(-1, -2)]
        public void TestEquality(float w, float h)
        {
            var size1 = new Size2d(w, h);
            var size2 = new Size2d(w, h);

            Assert.True(size1 == size2);
            Assert.True(size1.Equals(size2));

            size1.Width += Constants.FloatTolerance / 10;

            Assert.True(size1 == size2);
            Assert.True(size1.Equals(size2));

            size1.Width += Constants.FloatTolerance * 10;

            Assert.False(size1 == size2);
            Assert.False(size1.Equals(size2));
        }

        [Fact]
        public void TestHashCode()
        {
            var size1 = new Size2d(1, 2);
            var size2 = new Size2d(1, 2);

            Assert.Equal(size1.GetHashCode(), size2.GetHashCode());

            size2.Scale(1.1f, 1.1f);

            Assert.NotEqual(size1.GetHashCode(), size2.GetHashCode());
        }
    }
}
