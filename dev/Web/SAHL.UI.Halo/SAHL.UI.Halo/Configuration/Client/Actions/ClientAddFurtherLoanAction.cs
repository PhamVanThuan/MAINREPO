using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Actions
{
    public class ClientAddFurtherLoanAction : HaloTileActionEditBase<ClientRootTileConfiguration>,
                                              IHaloTileActionEdit<ClientRootTileConfiguration>
    {
        public ClientAddFurtherLoanAction()
            : base("Futher Loan", iconName: "icon-plus-2", actionGroup: "origination", sequence: 2)
        {
        }
    }
}