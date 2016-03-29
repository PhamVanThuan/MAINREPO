using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.UI.Halo.Models.Common.LoanTransaction;
using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.Account.LoanTransaction
{
    public class LoanTransactionsRootTileHeaderTextProvider :  HaloTileHeaderTextProviderBase<LoanTransactionsRootTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as LoanTransactionTileModel;
            if (model == null)
            { return; }
            this.HeaderText = model.TransactionTypeDescription;
        }
    }
}
