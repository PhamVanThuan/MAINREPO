using SAHL.UI.Halo.Models.Account;
using SAHL.UI.Halo.Shared.Configuration.Header;
using System;

namespace SAHL.UI.Halo.Configuration.Account.HOC
{
    public class HOCRootTileHeaderTextProvider : HaloTileHeaderTextProviderBase<HOCRootTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as HOCRootModel;
            if (model == null)
            {
                throw new Exception("Unexpected Data Model received");
            }

            this.HeaderText = model.AccountNumber;
        }
    }
}