using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Details
{
    public class AggregatedThirdPartyInvoicesDetailsChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<AggregatedThirdPartyInvoicesDetailsChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as AggregatedThirdPartyInvoiceGroupedModel;
            if (model == null)
            { return; }
            this.HeaderText = model.InvoiceStatusDescription;
        }
    }
}