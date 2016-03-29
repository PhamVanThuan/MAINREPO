using SAHL.UI.Halo.Models.Common.ArrearTransactions;
using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.Account.ArrearTransaction
{
    public class ArrearTransactionsRootTileHeaderTextProvider : HaloTileHeaderTextProviderBase<ArrearTransactionsRootTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as ArrearTransactionDetailModel;
            if (model == null)
            { return; }
            this.HeaderText = model.TransactionTypeDescription;
        }
    }
}