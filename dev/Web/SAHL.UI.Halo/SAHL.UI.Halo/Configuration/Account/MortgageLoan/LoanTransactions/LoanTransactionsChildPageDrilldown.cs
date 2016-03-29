using SAHL.UI.Halo.Configuration.Account.LoanTransaction;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.LoanTransactions
{
    public class LoanTransactionsChildPageDrilldown : HaloTileActionDrilldownBase<LoanTransactionsChildTileConfiguration, LoanTransactionsRootTileConfiguration>,
                                                    IHaloTileActionDrilldown<LoanTransactionsChildTileConfiguration>
    {
        public LoanTransactionsChildPageDrilldown()
            : base("Loan Transaction")
        {
        }
    }
}
