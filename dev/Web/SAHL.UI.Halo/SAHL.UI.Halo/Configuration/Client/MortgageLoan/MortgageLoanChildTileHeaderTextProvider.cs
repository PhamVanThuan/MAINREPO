using SAHL.UI.Halo.Models.Client.MortgageLoan;
using SAHL.UI.Halo.Shared.Configuration.Header;
using System;

namespace SAHL.UI.Halo.Configuration.Client.MortgageLoan
{
    public class MortgageLoanChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<MortgageLoanChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as MortgageLoanChildModel;
            if (model == null)
            {
                throw new Exception("Unexpected Data Model received");
            }

            this.HeaderText = model.AccountNumber;
        }
    }
}