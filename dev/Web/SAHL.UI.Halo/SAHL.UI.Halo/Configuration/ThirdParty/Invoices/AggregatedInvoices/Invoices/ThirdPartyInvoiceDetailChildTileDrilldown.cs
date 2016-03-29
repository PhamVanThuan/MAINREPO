using SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices
{
    public class ThirdPartyInvoiceDetailChildTileDrilldown : HaloTileActionDrilldownBase<ThirdPartyInvoiceDetailChildTileConfiguration,
                                                                                                     ThirdPartyInvoiceRootTileConfiguration>,
                                                                         IHaloTileActionDrilldown<ThirdPartyInvoiceDetailChildTileConfiguration>
    {
        public ThirdPartyInvoiceDetailChildTileDrilldown()
            : base("AggregatedThirdPartyInvoicesDetails")
        {
        }
    }
}