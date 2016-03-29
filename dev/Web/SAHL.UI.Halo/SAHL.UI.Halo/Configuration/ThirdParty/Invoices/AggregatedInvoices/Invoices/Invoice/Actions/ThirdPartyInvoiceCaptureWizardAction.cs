
using SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.Actions
{
    public class ThirdPartyInvoiceCaptureWizardAction : HaloTileWizardActionBase<ThirdPartyInvoiceRootTileConfiguration, ThirdPartyInvoiceCaptureWizardConfiguration>
    {
        public ThirdPartyInvoiceCaptureWizardAction(string contextData = null)
            : base("Capture Invoice", "icon-plus-2", "Capture", 1, contextData)
        {
        }
    }
}
