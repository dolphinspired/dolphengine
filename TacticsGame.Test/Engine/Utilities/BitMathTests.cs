using System;
using System.Collections.Generic;
using System.Text;
using TacticsGame.Engine.Utilities;
using Xunit;

namespace TacticsGame.Test.Engine.Utilities
{
    public class BitMathTests
    {
        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(7, 1)]
        [InlineData(8, 2)]
        [InlineData(9, 2)]
        [InlineData(15, 2)]
        [InlineData(16, 3)]
        [InlineData(17, 3)]
        public void CanGetBytesFromBitPosition(ushort bitPosition, ushort expectedNumBytes)
        {
            var numBytes = BitMath.NumBytesToReachBitPosition(bitPosition);
            Assert.Equal(expectedNumBytes, numBytes);
        }
    }
}
