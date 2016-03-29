using SAHL.UI.Halo.Models.Common.BankAccount;
using SAHL.UI.Halo.Shared.Configuration.Header;
using System;

namespace SAHL.UI.Halo.Configuration.Client.Detail.BankAccount
{
    public class ClientBankAccountChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<BankAccountChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as BankAccountChildModel;
            if (model == null)
            {
                throw new Exception("Unexpected Data Model received");
            }
            this.HeaderText = "Bank Account";
        }
    }
}