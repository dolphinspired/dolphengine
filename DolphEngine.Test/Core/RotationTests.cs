using Xunit;

namespace DolphEngine.Test.Core
{
    public class RotationTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(90)]
        [InlineData(179)]
        [InlineData(316.5)]
        [InlineData(488.25)]
        public void CanGetFromDegrees(float degrees)
        {
            var rot = Rotation2d.FromDegrees(degrees);
            DolphAssert.EqualF(degrees, rot.Degrees);
        }

        [Theory]
        [InlineData(   2,     2,     4)]
        [InlineData(   2,    -6,    -4)]
        [InlineData( 0.1,  0.25,  0.35)]
        [InlineData( 0.1, -0.25, -0.15)]
        public void CanTurnByRadians(float r1, float radians, float r2)
        {
            var rot = new Rotation2d(r1);
            rot.Turn(radians);

            DolphAssert.EqualF(r2, rot.Radians);
        }

        [Theory]
        [InlineData(  10,   40,   50)]
        [InlineData(  10,  -40,  -30)]
        [InlineData( -10,   40,   30)]
        [InlineData( -10,  -40,  -50)]
        [InlineData( 350,   70,  420)]
        [InlineData(-270,  -99, -369)]
        [InlineData(  90,  0.1, 90.1)]
        [InlineData(  90, -0.1, 89.9)]
        public void CanTurnByDegrees(float r1, float degrees, float r2)
        {
            var rot = Rotation2d.FromDegrees(r1);
            rot.TurnDegrees(degrees);

            DolphAssert.EqualF(r2, rot.Degrees);
        }

        [Fact]
        public void CanTurnByRotation()
        {
            var rot1 = Rotation2d.FromDegrees(45);
            var rot2 = Rotation2d.FromDegrees(90);

            rot1.Turn(rot2);

            DolphAssert.EqualF(135, rot1.Degrees);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(360)]
        [InlineData(720)]
        [InlineData(-180)]
        public void TestEquality(float radians)
        {
            var rot1 = new Rotation2d(radians);
            var rot2 = new Rotation2d(radians);

            Assert.True(rot1 == rot2);
            Assert.True(rot1.Equals(rot2));

            rot1.Radians += Constants.FloatTolerance / 10;

            Assert.True(rot1 == rot2);
            Assert.True(rot1.Equals(rot2));

            rot1.Radians += Constants.FloatTolerance * 10;

            Assert.False(rot1 == rot2);
            Assert.False(rot1.Equals(rot2));
        }

        [Fact]
        public void TestHashCode()
        {
            var rot1 = new Rotation2d(5);
            var rot2 = new Rotation2d(5);

            Assert.Equal(rot1.GetHashCode(), rot2.GetHashCode());

            rot2.Turn(0.1f);

            Assert.NotEqual(rot1.GetHashCode(), rot2.GetHashCode());
        }
    }
}
