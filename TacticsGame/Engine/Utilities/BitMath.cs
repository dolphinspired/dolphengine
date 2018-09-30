namespace TacticsGame.Engine.Utilities
{
    public static class BitMath
    {
        public static ushort NumBytesToReachBitPosition(ushort bitPosition)
        {
            return (ushort)((bitPosition / 8) + 1);
        }
    }
}
