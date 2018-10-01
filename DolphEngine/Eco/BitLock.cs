using System.Collections.Generic;

namespace DolphEngine.Eco
{
    public class BitLock
    {
        public BitLock(IEnumerable<ushort> bitPositions)
        {
            // Define the lock by creating a throwaway sample key that would fit this lock
            this.Bits = new BitKey(bitPositions).Bits;
            this.BitPositions = bitPositions;
        }

        public readonly IReadOnlyList<uint> Bits;

        public readonly IEnumerable<ushort> BitPositions;
    }
}
