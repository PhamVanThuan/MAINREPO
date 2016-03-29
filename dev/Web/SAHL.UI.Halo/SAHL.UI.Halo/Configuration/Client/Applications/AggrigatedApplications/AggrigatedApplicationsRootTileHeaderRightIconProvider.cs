
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Applications.AggregatedApplications
{
    public class AggregatedApplicationsRootTileHeaderRightIconProvider : HaloTileHeaderIconProviderBase<AggregatedApplicationsRootTileHeaderConfiguration>
    {
        public AggregatedApplicationsRootTileHeaderRightIconProvider()
            : base(HaloTileIconAlignment.Right)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            // No action required
        }
    }
}