using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Account.MortgageLoan.LoanTransactions;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Common.LoanTransactions
{
    public class LoanTransactionsChildDataProvider : HaloTileBaseChildDataProvider,
                                         IHaloTileChildDataProvider<LoanTransactionsChildTileConfiguration>
    {
        public LoanTransactionsChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
    }
}