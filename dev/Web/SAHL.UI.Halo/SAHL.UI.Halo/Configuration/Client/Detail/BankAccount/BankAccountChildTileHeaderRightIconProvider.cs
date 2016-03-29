using SAHL.UI.Halo.Models.Client.MortgageLoan;
using SAHL.UI.Halo.Models.Common.BankAccount;
using SAHL.UI.Halo.Shared.Configuration;
using System;

namespace SAHL.UI.Halo.Configuration.Client.Detail.BankAccount
{
    public class BankAccountChildTileHeaderRightIconProvider : HaloTileHeaderIconProviderBase<BankAccountChildTileHeaderConfiguration>
    {
        public BankAccountChildTileHeaderRightIconProvider()
            : base(HaloTileIconAlignment.Right)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            BankAccountChildModel bankAccountModel = dataModel as BankAccountChildModel;
            if (bankAccountModel == null)
            {
                throw new Exception("Unexpected Data Model received");
            }

            this.HeaderIcons.Add(string.Format("icon-bank-{0}", bankAccountModel.BankID).ToLower());
        }
    }
}