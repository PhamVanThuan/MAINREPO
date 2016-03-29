using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.ArrearTransactions
{
    public class ArrearTransactionsChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<ArrearTransactionsChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = "Arrear Transactions";
        }
    }
}