using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices
{
    public class ThirdPartyInvoiceDetailRootTileConfiguration : HaloSubTileConfiguration,
                                                     IHaloRootTileConfiguration,
                                                     IHaloModuleTileConfiguration<ThirdPartyHomeConfiguration>,
                                                     IHaloTileModel<ThirdPartyInvoiceDetailRootModel>
    {
        public ThirdPartyInvoiceDetailRootTileConfiguration()
            : base("Invoices", "ThirdPartyInvoiceDetails", 2)
        {
        }
    }
}