using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Actions
{
    public class ClientAddITCAction : HaloTileActionEditBase<ClientRootTileConfiguration>,
                                           IHaloTileActionEdit<ClientRootTileConfiguration>
    {
        public ClientAddITCAction()
            : base("ITC", iconName: "icon-plus-2", actionGroup: "client", sequence: 1)
        {
        }
    }
}