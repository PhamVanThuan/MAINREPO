using SAHL.UI.Halo.Models.Client;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client
{
    public class ClientRootTileConfiguration : HaloSubTileConfiguration,
                                               IHaloRootTileConfiguration,
                                               IHaloModuleTileConfiguration<ClientHomeConfiguration>,
                                               IHaloTileModel<ClientRootModel>,
                                               IHaloWorkflowTileActionProvider<HaloWorkflowTileActionProvider>
    {
        public ClientRootTileConfiguration()
            : base("Client", "Client", 1)
        {
        }
    }
}
