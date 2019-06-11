using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DolphEngine.Test.Core
{
    public class VectorTests
    {
        [Fact]
        public void CanSetVector()
        {
            var vec = new Vector2d(1, 2);
            vec.Set(3, 4);

            DolphAssert.EqualF(3, vec.X);
            DolphAssert.EqualF(4, vec.Y);
        }

        [Fact]
        public void CanConvertToPosition()
        {
            var vec = new Vector2d(1, 2);
            var pos = vec.ToPosition();

            DolphAssert.EqualF(vec.X, pos.X);
            DolphAssert.EqualF(vec.Y, pos.Y);
        }

        [Theory]
        [InlineData( 0,  0)]
        [InlineData( 1,  2)]
        [InlineData(-1,  2)]
        [InlineData( 1, -2)]
        [InlineData(-1, -2)]
        public void TestEquality(float x, float y)
        {
            var vec1 = new Vector2d(x, y);
            var vec2 = new Vector2d(x, y);

            Assert.True(vec1 == vec2);
            Assert.True(vec1.Equals(vec2));

            vec1.X += Constants.FloatTolerance / 10;

            Assert.True(vec1 == vec2);
            Assert.True(vec1.Equals(vec2));

            vec1.X += Constants.FloatTolerance * 10;

            Assert.False(vec1 == vec2);
            Assert.False(vec1.Equals(vec2));
        }

        [Theory]
        [InlineData( 1,  2,  3,  4,  4,  6)]
        [InlineData(-1, -2,  3,  4,  2,  2)]
        [InlineData( 1,  2, -3, -4, -2, -2)]
        [InlineData(-1, -2, -3, -4, -4, -6)]
        public void TestAdd(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            var vec = new Vector2d(x1, y1);
            vec += new Vector2d(x2, y2);

            DolphAssert.EqualF(x3, vec.X);
            DolphAssert.EqualF(y3, vec.Y);
        }

        [Theory]
        [InlineData( 1,  2,  3,  4, -2, -2)]
        [InlineData(-1, -2,  3,  4, -4, -6)]
        [InlineData( 1,  2, -3, -4,  4,  6)]
        [InlineData(-1, -2, -3, -4,  2,  2)]
        public void TestSubtract(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            var vec = new Vector2d(x1, y1);
            vec -= new Vector2d(x2, y2);

            DolphAssert.EqualF(x3, vec.X);
            DolphAssert.EqualF(y3, vec.Y);
        }

        [Theory]
        [InlineData( 20,  30,     4,   -2,  80, -60)]
        [InlineData( 20,  30, -0.25,  0.5,  -5,  15)]
        [InlineData(-20, -30, -0.25,  0.5,   5, -15)]
        public void TestMultiply(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            var vec = new Vector2d(x1, y1);
            vec *= new Vector2d(x2, y2);

            DolphAssert.EqualF(x3, vec.X);
            DolphAssert.EqualF(y3, vec.Y);
        }
        
        [Theory]
        [InlineData( 20,  30,     4,   -2,   5, -15)]
        [InlineData( 20,  30, -0.25,  0.5, -80,  60)]
        [InlineData(-20, -30, -0.25,  0.5,  80, -60)]
        public void TestDivide(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            var vec = new Vector2d(x1, y1);
            vec /= new Vector2d(x2, y2);

            DolphAssert.EqualF(x3, vec.X);
            DolphAssert.EqualF(y3, vec.Y);
        }

        [Theory]
        [InlineData( 0,  0,  0,  0)]
        [InlineData( 1,  1, -1, -1)]
        [InlineData( 1, -1, -1,  1)]
        [InlineData(-1,  1,  1, -1)]
        [InlineData(-1, -1,  1,  1)]
        public void TestNegative(float x1, float y1, float x2, float y2)
        {
            var vec = new Vector2d(x1, y1);
            vec = -vec;

            DolphAssert.EqualF(x2, vec.X);
            DolphAssert.EqualF(y2, vec.Y);
        }

        [Fact]
        public void TestHashCode()
        {
            var vec1 = new Vector2d(1, 2);
            var vec2 = new Vector2d(1, 2);

            Assert.Equal(vec1.GetHashCode(), vec2.GetHashCode());

            vec2.Set(1, 1);

            Assert.NotEqual(vec1.GetHashCode(), vec2.GetHashCode());
        }
    }
}
