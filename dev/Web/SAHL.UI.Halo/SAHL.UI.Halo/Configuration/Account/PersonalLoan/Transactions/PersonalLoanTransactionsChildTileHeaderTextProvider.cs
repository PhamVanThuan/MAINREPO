using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.Account.PersonalLoan.Transactions
{
    public class PersonalLoanTransactionsChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<PersonalLoanTransactionsChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = "Personal Loan Transactions";
        }
    }
}