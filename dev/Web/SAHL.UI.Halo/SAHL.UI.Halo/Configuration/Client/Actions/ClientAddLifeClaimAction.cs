
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Actions
{
    public class ClientAddLifeClaimAction : HaloTileActionEditBase<ClientRootTileConfiguration>,
                                           IHaloTileActionEdit<ClientRootTileConfiguration>
    {
        public ClientAddLifeClaimAction()
            : base("Life Claim", iconName: "icon-plus-2", actionGroup: "support", sequence: 2)
        {
        }
    }
}