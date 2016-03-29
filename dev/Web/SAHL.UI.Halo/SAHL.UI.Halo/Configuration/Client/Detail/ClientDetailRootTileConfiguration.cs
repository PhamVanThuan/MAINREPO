using SAHL.UI.Halo.Models.Client;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Detail
{
    public class ClientDetailRootTileConfiguration : HaloSubTileConfiguration,
                                                     IHaloRootTileConfiguration,
                                                     IHaloModuleTileConfiguration<ClientHomeConfiguration>,
                                                     IHaloTileModel<ClientRootModel>
    {
        public ClientDetailRootTileConfiguration()
            : base("Detail", "ClientDetail")
        {
        }
    }
}