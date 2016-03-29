using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Details
{
    public class AggregatedThirdPartyInvoicesDetailsChildTileDrilldown : HaloTileActionDrilldownBase<AggregatedThirdPartyInvoicesDetailsChildTileConfiguration, 
                                                                                                     ThirdPartyInvoiceDetailRootTileConfiguration>,
                                                                         IHaloTileActionDrilldown<AggregatedThirdPartyInvoicesDetailsChildTileConfiguration>
    {
        public AggregatedThirdPartyInvoicesDetailsChildTileDrilldown()
            : base("AggregatedThirdPartyInvoicesDetails")
        {
        }
    }
}