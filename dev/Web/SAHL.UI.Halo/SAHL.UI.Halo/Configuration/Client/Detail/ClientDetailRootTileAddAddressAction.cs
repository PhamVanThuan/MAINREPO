using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Detail
{
    public class ClientDetailRootTileAddAddressAction : HaloTileActionEditBase<ClientDetailRootTileConfiguration>,
                                                        IHaloTileActionEdit<ClientDetailRootTileConfiguration>
    {
        public ClientDetailRootTileAddAddressAction()
            : base("Address", iconName: "icon-plus-2", sequence: 2)
        {
        }
    }
}