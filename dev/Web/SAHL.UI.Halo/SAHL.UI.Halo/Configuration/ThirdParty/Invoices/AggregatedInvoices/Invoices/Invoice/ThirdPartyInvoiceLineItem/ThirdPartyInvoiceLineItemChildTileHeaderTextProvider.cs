using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.ThirdPartyInvoiceLineItem
{
    public class ThirdPartyInvoiceLineItemChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<ThirdPartyInvoiceLineItemChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = "Invoice Line Items";
        }
    }
}