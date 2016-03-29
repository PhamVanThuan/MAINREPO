using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Client.Applications.AggregatedApplications;
using SAHL.UI.Halo.Models.Client;
using SAHL.UI.Halo.Models.Client.Applications;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Client.Applications.AggregatedApplications
{
    public class AggregatedApplicationsRootDataProvider : HaloTileBaseChildDataProvider,
                                                        IHaloTileChildDataProvider<AggregatedApplicationsRootTileConfiguration>
    {
        public AggregatedApplicationsRootDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
    }
}