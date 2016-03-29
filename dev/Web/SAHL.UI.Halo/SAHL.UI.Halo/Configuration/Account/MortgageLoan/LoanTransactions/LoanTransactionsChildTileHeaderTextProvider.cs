using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.LoanTransactions
{
    public class LoanTransactionsChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<LoanTransactionsChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = "Loan Transactions";
        }
    }
}