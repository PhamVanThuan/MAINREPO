using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Models.ThirdParty.Invoices;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice
{
    public class ThirdPartyInvoiceRootTileConfiguration : HaloSubTileConfiguration,
                                                          IHaloRootTileConfiguration,
                                                          IHaloModuleTileConfiguration<ThirdPartyHomeConfiguration>,
                                                          IHaloModuleTileConfiguration<TaskHomeConfiguration>,
                                                          IHaloTileModel<ThirdPartyInvoiceRootModel>,
                                                          IHaloWorkflowTileActionProvider<HaloWorkflowTileActionProvider>
    {
        public ThirdPartyInvoiceRootTileConfiguration()
            : base("Invoice", "ThirdPartyInvoice", sequence: 4)
        {
        }
    }
}