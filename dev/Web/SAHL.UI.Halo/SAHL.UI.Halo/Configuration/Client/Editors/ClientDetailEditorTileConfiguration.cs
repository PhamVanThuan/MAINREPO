using SAHL.UI.Halo.Configuration.Client.Detail;
using SAHL.UI.Halo.Models.Client;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client
{
    public class ClientDetailEditorTileConfiguration : HaloBaseTileConfiguration,
                                                       IHaloTileEditorConfiguration<ClientRootTileConfiguration>,
                                                       IHaloTileModel<ClientRootModel>
    {
        public ClientDetailEditorTileConfiguration()
            : base("Client Detail", "ClientDetail")
        {
        }
    }
}