using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.InvoiceDocument
{
    public class InvoiceDocumentChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<InvoiceDocumentChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = "";
        }
    }
}