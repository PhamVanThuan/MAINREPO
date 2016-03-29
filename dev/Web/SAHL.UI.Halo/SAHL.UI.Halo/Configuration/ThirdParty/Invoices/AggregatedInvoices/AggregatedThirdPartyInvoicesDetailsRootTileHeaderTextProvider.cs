using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Details
{
    public class AggregatedThirdPartyInvoicesDetailsRootTileHeaderTextProvider : HaloTileHeaderTextProviderBase<AggregatedThirdPartyInvoicesDetailsRootTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as AggregatedThirdPartyInvoiceRootModel;
            if (model == null)
            { return; }
            this.HeaderText = model.LegalName;
        }
    }
}