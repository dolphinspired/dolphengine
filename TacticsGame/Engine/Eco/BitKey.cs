using System.Collections.Generic;
using System.Linq;

namespace TacticsGame.Engine.Eco
{
    public class BitKey
    {
        /// <summary>
        /// The number of bits in the primitive that backs this key.
        /// A byte would be 8, an int 32, etc.
        /// </summary>
        private const int BitSegmentLength = 32;

        public BitKey(IEnumerable<ushort> bitPositions)
        {
            if (bitPositions == null || !bitPositions.Any())
            {
                this.Bits = new uint[0];
                return;
            }
            
            var bits = new uint[(bitPositions.Max() / BitSegmentLength) + 1];
            if (bitPositions != null && bitPositions.Any())
            {
                foreach (var bitPos in bitPositions)
                {
                    var byteIndex = bitPos / BitSegmentLength;    // Arbitrarily large
                    var bitIndex = bitPos % BitSegmentLength;     // Range of 0-{BitSegmentLength-1}

                    // Intersect the selected byte with a mask created from the selected bit
                    // This will preserve whatever is already in the byte, while flipping the bitIndex to 1 if it's not already
                    bits[byteIndex] |= (uint)(1 << bitIndex);
                }
            }            

            this.Bits = bits;
        }

        public readonly IReadOnlyList<uint> Bits;

        public bool Unlocks(BitLock bitLock)
        {
            var totalKeySegments = this.Bits.Count;
            var totalLockSegments = bitLock.Bits.Count;
            
            if (totalLockSegments > totalKeySegments)
            {
                // If the lock is larger than the key, look first at all lock bytes past the key's length
                var farLock = bitLock.Bits.Skip(totalKeySegments);
                if (farLock.Any(x => x > 0))
                {
                    // If any lock byte past the key's length has any bits flipped to 1, this key cannot possibly fit
                    return false;
                }
            }

            // Only look at the bytes of the key that have corresponding lock bytes
            // i.e. if the key is longer than the lock, trim down the key to fit the lock
            for (var segmentPos = 0; segmentPos < totalLockSegments; segmentPos++)
            {
                // Example:
                //  Key: 10010100
                // Lock: 10110011
                //       O XO  XX - passes 2 bits, fails 3 bits. Does NOT unlock.
                //  Key: 10010100
                // Lock: 10000100
                //       O    O   - passes 2 bits. Unlock successful. Don't care about extra key bits.

                var lockSegment = bitLock.Bits[segmentPos];
                var keySegment = this.Bits[segmentPos];

                if ((keySegment & lockSegment) != lockSegment)
                {
                    return false;
                }
            }

            // If no bytes failed, then the unlock is successful
            return true;
        }
    }
}
