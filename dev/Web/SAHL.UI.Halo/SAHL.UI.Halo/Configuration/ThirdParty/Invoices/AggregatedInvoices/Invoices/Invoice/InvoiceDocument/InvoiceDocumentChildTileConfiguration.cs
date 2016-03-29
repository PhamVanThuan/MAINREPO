using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.InvoiceDocument
{
    public class InvoiceDocumentChildTileConfiguration :    HaloSubTileConfiguration,
                                                            IHaloChildTileConfiguration<ThirdPartyInvoiceRootTileConfiguration>,
                                                            IHaloTileModel<InvoiceDocumentChildModel>
    {
        public InvoiceDocumentChildTileConfiguration()
            : base("Invoices", "ThirdPartyInvoices", sequence: 2, startRow: 1, noOfRows: 1, noOfColumns: 1)
        {
        }
    }
}