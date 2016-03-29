using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice
{
    public class ThirdPartyInvoiceLineItemChildTileConfiguration : HaloSubTileConfiguration,
                                                    IHaloChildTileConfiguration<ThirdPartyInvoiceRootTileConfiguration>,
                                                    IHaloTileModel<ThirdPartyInvoiceLineItemModel>
    {
        public ThirdPartyInvoiceLineItemChildTileConfiguration()
            : base("Third Party Invoice Line Item", "ThirdPartyInvoiceLineItem", 1, startRow: 1, noOfRows: 4, noOfColumns: 4)
        {
        }
    }
}