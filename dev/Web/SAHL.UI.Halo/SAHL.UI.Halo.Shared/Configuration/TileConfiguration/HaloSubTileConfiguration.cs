namespace SAHL.UI.Halo.Shared.Configuration
{
    public abstract class HaloSubTileConfiguration : HaloBaseTileConfiguration, IHaloSubTileConfiguration
    {
        protected HaloSubTileConfiguration(string tileName, string tileType, int sequence = 0, bool isTileBased = true,
                                           int startRow = 0, int startColumn = 0, int noOfRows = 1, int noOfColumns = 1) 
            : base(tileName, tileType, sequence, isTileBased)
        {
            this.StartRow    = startRow;
            this.StartColumn = startColumn;
            this.NoOfRows    = noOfRows;
            this.NoOfColumns = noOfColumns;
        }

        public int StartRow { get; private set; }
        public int StartColumn { get; private set; }
        public int NoOfRows { get; private set; }
        public int NoOfColumns { get; private set; }
    }
}
