using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Actions
{
    public class ClientAddPersonalLoanAction : HaloTileActionEditBase<ClientRootTileConfiguration>,
                                               IHaloTileActionEdit<ClientRootTileConfiguration>
    {
        public ClientAddPersonalLoanAction()
            : base("Personal Loan", iconName: "icon-plus-2", actionGroup: "origination", sequence: 3)
        {
        }
    }
}