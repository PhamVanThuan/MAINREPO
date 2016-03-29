using System;
using SAHL.UI.Halo.Models.Account;
using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan
{
    public class MortgageLoanRootTileHeaderTextProvider : HaloTileHeaderTextProviderBase<MortgageLoanRootTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as MortgageLoanRootModel;
            if (model == null) { return; }

            this.HeaderText = model.AccountNumber;
        }
    }
}