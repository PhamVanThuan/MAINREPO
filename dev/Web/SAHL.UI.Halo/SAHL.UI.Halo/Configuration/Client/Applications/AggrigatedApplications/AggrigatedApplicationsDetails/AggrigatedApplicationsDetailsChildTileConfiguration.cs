using SAHL.UI.Halo.Configuration.Client.Applications.Application;
using SAHL.UI.Halo.Models.Client.Applications;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Applications.AggregatedApplications.AggregatedApplicationsDetails
{
    public class AggregatedApplicationsDetailsChildTileConfiguration : HaloSubTileConfiguration,
                                                    IHaloChildTileConfiguration<AggregatedApplicationsRootTileConfiguration>,
                                                    IHaloTileModel<AggregatedApplicationsDetailsModel>
    {
        public AggregatedApplicationsDetailsChildTileConfiguration()
            : base("Applications", "Applications", 1, noOfRows: 2, noOfColumns: 3)
        {
        }
    }
}