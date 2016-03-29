using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Actions
{
    public class ClientAddHelpdeskAction : HaloTileActionEditBase<ClientRootTileConfiguration>,
                                           IHaloTileActionEdit<ClientRootTileConfiguration>
    {
        public ClientAddHelpdeskAction()
            : base("Helpdesk", iconName: "icon-plus-2", actionGroup: "support", sequence: 1)
        {
        }
    }
}