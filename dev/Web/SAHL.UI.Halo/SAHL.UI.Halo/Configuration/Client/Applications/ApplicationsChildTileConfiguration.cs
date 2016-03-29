using SAHL.UI.Halo.Models.Client.Applications;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Applications
{
    public class ApplicationsChildTileConfiguration : HaloSubTileConfiguration,
                                                      IHaloChildTileConfiguration<ClientRootTileConfiguration>,
                                                      IHaloTileModel<ApplicationsChildModel>
    {
        public ApplicationsChildTileConfiguration()
            : base("Applications", "Applications", 8, noOfRows: 1, noOfColumns: 1)
        {
        }
    }
}