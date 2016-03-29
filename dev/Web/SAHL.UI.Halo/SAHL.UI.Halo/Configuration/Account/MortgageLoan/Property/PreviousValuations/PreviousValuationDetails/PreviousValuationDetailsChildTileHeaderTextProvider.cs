using SAHL.UI.Halo.Models.Account.Property;
using SAHL.UI.Halo.Shared.Configuration.Header;
using System;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.PreviousValuations.PreviousValuationDetails
{
    public class PreviousValuationDetailsChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<PreviousValuationDetailsChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as PreviousValuationModel;
            if (model == null)
            {
                throw new Exception("Unexpected Data Model received");
            }

            this.HeaderText = model.ValuationAmount.ToString("C0") + " (" + model.ValuationAge + " old)";
        }
    }
}