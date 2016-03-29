using SAHL.UI.Halo.Models.Common.LoanTransaction;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.LoanTransactions
{
    public class LoanTransactionsChildTileConfiguration : HaloSubTileConfiguration,
                                                            IHaloChildTileConfiguration<MortgageLoanRootTileConfiguration>,
                                                            IHaloTileModel<LoanTransactionTileModel>
    {
        public LoanTransactionsChildTileConfiguration()
            : base("Loan Transactions", "LoanTransactions", 2, noOfRows: 2, noOfColumns: 4)
        {
        }
    }
}