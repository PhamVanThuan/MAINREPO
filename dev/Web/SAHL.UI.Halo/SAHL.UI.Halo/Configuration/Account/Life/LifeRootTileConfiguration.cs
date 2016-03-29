using SAHL.UI.Halo.Models.Account;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.Life
{
    public class LifeRootTileConfiguration : HaloSubTileConfiguration,
                                            IHaloRootTileConfiguration,
                                            IHaloModuleTileConfiguration<ClientHomeConfiguration>,
                                            IHaloTileModel<LifeRootModel>,
                                            IHaloWorkflowTileActionProvider<HaloWorkflowTileActionProvider>
    {
        public LifeRootTileConfiguration()
            : base("Life", "Life", 2)
        {
        }
    }
}