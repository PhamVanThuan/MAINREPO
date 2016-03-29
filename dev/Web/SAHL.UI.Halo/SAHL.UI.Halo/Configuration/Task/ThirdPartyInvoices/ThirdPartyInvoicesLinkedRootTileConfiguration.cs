
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration.LinkedRootTileConfiguration;
using SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice;

namespace SAHL.UI.Halo.Configuration.Task.ThirdPartyInvoices
{
    [HaloTileLinked("Third Party Invoices", "Third Party Invoices")]
    public class ThirdPartyInvoicesLinkedRootTileConfiguration : HaloRootTileLinkConfiguration,
                                                                 IHaloRootTileLinkedConfiguration<ThirdPartyInvoiceRootTileConfiguration>,
                                                                 IHaloModuleTileConfiguration<TaskHomeConfiguration>
    {
    }
}
