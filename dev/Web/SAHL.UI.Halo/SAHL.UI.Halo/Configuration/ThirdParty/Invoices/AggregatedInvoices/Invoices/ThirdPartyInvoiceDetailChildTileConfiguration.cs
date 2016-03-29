using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices
{
    public class ThirdPartyInvoiceDetailChildTileConfiguration : HaloSubTileConfiguration,
                                                    IHaloChildTileConfiguration<ThirdPartyInvoiceDetailRootTileConfiguration>,
                                                    IHaloTileModel<ThirdPartyInvoiceDetailModel>
    {
        public ThirdPartyInvoiceDetailChildTileConfiguration()
            : base("Third Party Invoice Detail", "ThirdPartyInvoiceDetail", 1, noOfRows: 2, noOfColumns: 3)
        {
        }
    }
}