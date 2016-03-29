
using SAHL.UI.Halo.Configuration.Account.PersonalLoanTransactions;
using SAHL.UI.Halo.Shared.Configuration;
namespace SAHL.UI.Halo.Configuration.Account.PersonalLoan.Transactions
{
    public class PersonalLoanTransactionsChildPageDrilldown : HaloTileActionDrilldownBase<PersonalLoanTransactionsChildTileConfiguration, PersonalLoanTransactionsRootTileConfiguration>,
                                                    IHaloTileActionDrilldown<PersonalLoanTransactionsChildTileConfiguration>
    {
        public PersonalLoanTransactionsChildPageDrilldown()
            : base("Personal Loan Transaction")
        {

        }
    }
}
