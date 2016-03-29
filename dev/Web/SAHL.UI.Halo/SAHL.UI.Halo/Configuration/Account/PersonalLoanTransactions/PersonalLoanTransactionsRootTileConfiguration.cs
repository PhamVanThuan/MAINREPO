using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Models.Common.LoanTransaction;
using SAHL.UI.Halo.Configuration.Account.LoanTransaction;
using SAHL.UI.Halo.Pages;
using SAHL.UI.Halo.Pages.Common.Transactions;

namespace SAHL.UI.Halo.Configuration.Account.PersonalLoanTransactions
{
    public class PersonalLoanTransactionsRootTileConfiguration : HaloSubTileConfiguration,
                                                                 IHaloRootTileConfiguration,
                                                                 IHaloModuleTileConfiguration<ClientHomeConfiguration>,
                                                                 IHaloTileModel<LoanTransactionDetailModel>,
                                                                 IHaloTilePageState<LoanTransactionsPageState>
    {
        public PersonalLoanTransactionsRootTileConfiguration()
            : base("Personal Loan Transaction", "PersonalLoanTransaction", 5, false, noOfRows: 5)
        {

        }
    }
}