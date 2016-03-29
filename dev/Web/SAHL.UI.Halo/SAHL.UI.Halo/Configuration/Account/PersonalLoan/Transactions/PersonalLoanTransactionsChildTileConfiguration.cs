using SAHL.UI.Halo.Models.Common.LoanTransaction;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.PersonalLoan.Transactions
{
    public class PersonalLoanTransactionsChildTileConfiguration : HaloSubTileConfiguration,
                                                      IHaloChildTileConfiguration<PersonalLoanRootTileConfiguration>,
                                                      IHaloTileModel<LoanTransactionTileModel>
    {
        public PersonalLoanTransactionsChildTileConfiguration()
            : base("Personal Loan", "PersonalLoan", 2, noOfRows: 2, noOfColumns: 4)
        {

        }
    }
}
