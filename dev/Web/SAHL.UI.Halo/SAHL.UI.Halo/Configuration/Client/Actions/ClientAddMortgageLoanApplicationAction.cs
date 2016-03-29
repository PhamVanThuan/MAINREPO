using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Actions
{
    public class ClientAddMortgageLoanApplicationAction : HaloTileActionEditBase<ClientRootTileConfiguration>,
                                           IHaloTileActionEdit<ClientRootTileConfiguration>
    {
        public ClientAddMortgageLoanApplicationAction()
            : base("Mortgage Loan Application", iconName: "icon-plus-2", actionGroup: "origination", sequence: 1)
        {
        }
    }
}