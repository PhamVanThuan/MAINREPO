using System;
using SAHL.UI.Halo.Configuration.Account.MortgageLoan.Actions;
using SAHL.UI.Halo.Models.Account;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan
{
    public class MortgageLoanRootTileConfiguration : HaloSubTileConfiguration,
                                                     IHaloRootTileConfiguration,
                                                     IHaloModuleTileConfiguration<ClientHomeConfiguration>,
                                                     IHaloTileModel<MortgageLoanRootModel>,
                                                     IHaloTileDynamicActionProvider<MortgageLoanDynamicActionProvider>,
                                                     IHaloWorkflowTileActionProvider<HaloWorkflowTileActionProvider>
    {
        public MortgageLoanRootTileConfiguration()
            : base("Mortgage Loan", "MortgageLoan", 2, noOfRows: 2)
        {
        }
    }
}
