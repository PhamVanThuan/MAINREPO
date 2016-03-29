using SAHL.UI.Halo.Models.Account;
using SAHL.UI.Halo.Shared.Configuration.Header;
using System;

namespace SAHL.UI.Halo.Configuration.Account.Life
{
    public class LifeRootTileHeaderTextProvider : HaloTileHeaderTextProviderBase<LifeRootTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as LifeRootModel;
            if (model == null)
            {
                throw new Exception("Unexpected Data Model received");
            }

            this.HeaderText = model.AccountNumber;
        }
    }
}