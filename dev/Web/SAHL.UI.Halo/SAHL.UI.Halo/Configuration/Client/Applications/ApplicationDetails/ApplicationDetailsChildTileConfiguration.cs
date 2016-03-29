using SAHL.UI.Halo.Configuration.Client.Applications.Application;
using SAHL.UI.Halo.Models.Client.Applications;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Applications.ApplicationDetails
{
    public class ApplicationDetailsChildTileConfiguration : HaloSubTileConfiguration,
                                                    IHaloChildTileConfiguration<ApplicationsRootTileConfiguration>,
                                                    IHaloTileModel<ApplicationDetailsModel>
    {
        public ApplicationDetailsChildTileConfiguration()
            : base("Application Details", "ApplicationDetails", 1, noOfRows: 2, noOfColumns: 3)
        {
        }
    }
}