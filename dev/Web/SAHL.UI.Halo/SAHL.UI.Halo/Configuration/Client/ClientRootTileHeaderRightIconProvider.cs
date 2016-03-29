using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client
{
    public class ClientRootTileHeaderRightIconProvider : HaloTileHeaderIconProviderBase<ClientRootTileHeaderConfiguration>
    {
        public ClientRootTileHeaderRightIconProvider()
            : base(HaloTileIconAlignment.Right)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            // No action required
        }
    }
}