using System;

using SAHL.UI.Halo.Models.Account;
using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Life
{
    public class LifeChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<LifeChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model       = dataModel as LifeChildModel;
            this.HeaderText = model == null ? "UNKNOWN" : model.AccountNumber;
        }
    }
}
