using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Models.Common.LoanTransaction;
using SAHL.UI.Halo.Pages;
using SAHL.UI.Halo.Pages.Common.Transactions;

namespace SAHL.UI.Halo.Configuration.Account.LoanTransaction
{
    public class LoanTransactionsRootTileConfiguration : HaloSubTileConfiguration,
                                                         IHaloRootTileConfiguration,
                                                         IHaloModuleTileConfiguration<ClientHomeConfiguration>,
                                                         IHaloTileModel<LoanTransactionTileModel>,
                                                         IHaloTilePageState<LoanTransactionsPageState>
    {
        public LoanTransactionsRootTileConfiguration()
            : base("Loan Transaction", "LoanTransaction", 5, false)
        {
        }
    }
}
