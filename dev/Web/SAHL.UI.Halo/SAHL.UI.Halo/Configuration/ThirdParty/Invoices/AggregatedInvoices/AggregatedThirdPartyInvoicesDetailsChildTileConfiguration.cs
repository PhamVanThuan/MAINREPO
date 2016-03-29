using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Details
{
    public class AggregatedThirdPartyInvoicesDetailsChildTileConfiguration : HaloSubTileConfiguration,
                                                                             IHaloChildTileConfiguration<AggregatedThirdPartyInvoicesDetailsRootTileConfiguration>,
                                                                             IHaloTileModel<AggregatedThirdPartyInvoiceGroupedModel>
    {
        public AggregatedThirdPartyInvoicesDetailsChildTileConfiguration()
            : base("Invoice Status", "AggregatedThirdPartyInvoicesDetails", 1, startRow: 1, noOfRows: 1, noOfColumns: 1)
        {
        }
    }
}