using SAHL.UI.Halo.Configuration;
using SAHL.UI.Halo.Models.Account.Property;
using SAHL.UI.Halo.Models.Client;
using SAHL.UI.Halo.Models.Client.Applications;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Applications.AggregatedApplications
{
    public class AggregatedApplicationsRootTileConfiguration : HaloSubTileConfiguration,
                                                     IHaloRootTileConfiguration,
                                                     IHaloModuleTileConfiguration<ClientHomeConfiguration>,
                                                     IHaloTileModel<AggregatedApplicationsRootModel>
    {
        public AggregatedApplicationsRootTileConfiguration()
            : base("Applications Per Year", "Applications Per Year", 4, noOfRows: 2)
        {
        }
    }
}