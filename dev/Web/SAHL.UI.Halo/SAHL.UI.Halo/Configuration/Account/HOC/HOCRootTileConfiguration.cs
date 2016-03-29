using SAHL.UI.Halo.Models.Account;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.HOC
{
    public class HOCRootTileConfiguration : HaloSubTileConfiguration,
                                            IHaloRootTileConfiguration,
                                            IHaloModuleTileConfiguration<ClientHomeConfiguration>,
                                            IHaloTileModel<HOCRootModel>,
                                            IHaloWorkflowTileActionProvider<HaloWorkflowTileActionProvider>
    {
        public HOCRootTileConfiguration()
            : base("HOC", "HOC", 2)
        {
        }
    }
}