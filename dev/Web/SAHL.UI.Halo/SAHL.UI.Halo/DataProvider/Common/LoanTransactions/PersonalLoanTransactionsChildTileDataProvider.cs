using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Account.PersonalLoan.Transactions;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Common.LoanTransactions
{
    public class PersonalLoanTransactionsChildTileDataProvider : HaloTileBaseChildDataProvider,
                                         IHaloTileChildDataProvider<PersonalLoanTransactionsChildTileConfiguration>
    {
        public PersonalLoanTransactionsChildTileDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

    }
}
