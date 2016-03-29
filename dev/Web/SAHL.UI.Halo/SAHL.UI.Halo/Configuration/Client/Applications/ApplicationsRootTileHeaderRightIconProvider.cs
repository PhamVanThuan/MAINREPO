
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Applications
{
    public class ApplicationsRootTileHeaderRightIconProvider : HaloTileHeaderIconProviderBase<ApplicationsRootTileHeaderConfiguration>
    {
        public ApplicationsRootTileHeaderRightIconProvider()
            : base(HaloTileIconAlignment.Right)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            // No action required
        }
    }
}