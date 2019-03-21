using Xunit;

namespace DolphEngine.Test.Core
{
    public class RotationTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(90)]
        [InlineData(179.000000)]
        [InlineData(316.5000000)]
        [InlineData(488.25000000)]
        public void CanGetSetDegrees(float degrees)
        {
            var rotation = Rotation2d.FromDegrees(degrees);
            DolphAssert.EqualF(degrees, rotation.Degrees);
        }
    }
}
