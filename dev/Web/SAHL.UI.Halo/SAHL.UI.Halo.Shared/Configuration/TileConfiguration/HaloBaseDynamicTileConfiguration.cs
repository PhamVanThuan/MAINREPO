using SAHL.Core.BusinessModel;

namespace SAHL.UI.Halo.Shared.Configuration.TileConfiguration
{
    public abstract class HaloBaseDynamicTileConfiguration : HaloSubTileConfiguration, IHaloDynamicRootTileConfiguration
    {
        protected HaloBaseDynamicTileConfiguration(string tileName, string tileType, int sequence = 0, bool isTileBased = true, 
                                                   int startRow = 0, int startColumn = 0, int noOfRows = 1, int noOfColumns = 1) 
            : base(tileName, tileType, sequence, isTileBased, startRow, startColumn, noOfRows, noOfColumns)
        {
        }

        public virtual IHaloRootTileConfiguration GetRootTileConfiguration(BusinessContext businessContext)
        {
            return null;
        }

        public virtual IHaloRootTileConfiguration GetRootTileConfiguration(HaloDynamicTileDataModel dynamicTileDataModel)
        {
            return null;
        }
    }
}
