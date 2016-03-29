using SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Details;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices
{
    public class ThirdPartyInvoicesChildTileDrilldown : HaloTileActionDrilldownBase<ThirdPartyInvoicesChildTileConfiguration, AggregatedThirdPartyInvoicesDetailsRootTileConfiguration>,
                                                        IHaloTileActionDrilldown<ThirdPartyInvoicesChildTileConfiguration>
    {
        public ThirdPartyInvoicesChildTileDrilldown()
            : base("ThirdPartyInvoices")
        {
        }
    }
}