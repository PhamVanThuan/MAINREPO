using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Details
{
    public class AggregatedThirdPartyInvoicesDetailsRootTileConfiguration : HaloSubTileConfiguration,
                                                                            IHaloRootTileConfiguration,
                                                                            IHaloModuleTileConfiguration<ThirdPartyHomeConfiguration>,
                                                                            IHaloTileModel<AggregatedThirdPartyInvoiceRootModel>
    {
        public AggregatedThirdPartyInvoicesDetailsRootTileConfiguration()
            : base("Aggregated Invoices", "AggregatedThirdPartyInvoicesDetails", 2)
        {
        }
    }
}