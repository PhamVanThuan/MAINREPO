using SAHL.UI.Halo.Pages.Common.Invoices;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.InvoiceDocument
{
    public class InvoiceDocumentRootTileConfiguration : HaloSubTileConfiguration,
                                                        IHaloRootTileConfiguration,
                                                        IHaloModuleTileConfiguration<ThirdPartyHomeConfiguration>,
                                                        IHaloTilePageState<InvoiceDocumentPageState>

    {
        public InvoiceDocumentRootTileConfiguration()
            : base("Invoice Document Details", "InvoiceDocumentDetails", 2, false)
        {
        }
    }
    
}