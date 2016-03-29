using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Detail
{
    public class ClientDetailRootTileAddBankAccountAction : HaloTileActionEditBase<ClientDetailRootTileConfiguration>,
                                                        IHaloTileActionEdit<ClientDetailRootTileConfiguration>
    {
        public ClientDetailRootTileAddBankAccountAction()
            : base("Bank Account", iconName: "icon-plus-2", sequence: 3)
        {
        }
    }
}