using SAHL.UI.Halo.Configuration;
using SAHL.UI.Halo.Models.Account.Property;
using SAHL.UI.Halo.Models.Client;
using SAHL.UI.Halo.Models.Client.Applications;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Applications
{
    public class ApplicationsRootTileConfiguration : HaloSubTileConfiguration,
                                                     IHaloRootTileConfiguration,
                                                     IHaloModuleTileConfiguration<ClientHomeConfiguration>,
                                                     IHaloTileModel<ApplicationRootModel>
    {
        public ApplicationsRootTileConfiguration()
            : base("Applications", "Applications", 4, noOfRows: 2)
        {
        }
    }
}