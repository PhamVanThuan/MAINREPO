using SAHL.UI.Halo.Configuration.Client.Applications.Application;
using SAHL.UI.Halo.Models.Client.Applications;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Applications.AggregatedApplications
{
    public class AggregatedApplicationsChildTileConfiguration : HaloSubTileConfiguration,
                                                    IHaloChildTileConfiguration<ApplicationsRootTileConfiguration>,
                                                    IHaloTileModel<AggregatedApplicationsModel>
    {
        public AggregatedApplicationsChildTileConfiguration()
            : base("Applications", "Applications", 8, noOfRows: 1, noOfColumns: 1)
        {
        }
    }
}