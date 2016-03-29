using SAHL.UI.Halo.Configuration.Client.Detail;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client
{
    public class ClientRootTileDrilldown : HaloTileActionDrilldownBase<ClientRootTileConfiguration, ClientDetailRootTileConfiguration>,
                                           IHaloTileActionDrilldown<ClientRootTileConfiguration>
    {
        public ClientRootTileDrilldown()
            : base("Client")
        {
        }
    }
}