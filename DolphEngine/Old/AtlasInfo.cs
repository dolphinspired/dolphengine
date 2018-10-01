namespace DolphEngine.Old
{
    public struct AtlasInfo
    {
        public AtlasInfo(int wide, int tall)
        {
            this.NumTilesWide = wide;
            this.NumTilesTall = tall;
            this.NumTiles = wide * tall;
        }

        public readonly int NumTilesWide;

        public readonly int NumTilesTall;

        public readonly int NumTiles;

        public override string ToString()
        {
            return $"{this.NumTilesWide},{this.NumTilesTall}";
        }
    }
}