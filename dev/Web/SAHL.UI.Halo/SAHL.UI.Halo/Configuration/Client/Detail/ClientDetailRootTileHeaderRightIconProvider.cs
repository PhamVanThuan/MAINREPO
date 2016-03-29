using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Detail
{
    public class ClientDetailRootTileHeaderRightIconProvider : HaloTileHeaderIconProviderBase<ClientDetailRootTileHeaderConfiguration>
    {
        public ClientDetailRootTileHeaderRightIconProvider()
            : base(HaloTileIconAlignment.Right)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            // No action required
        }
    }
}
