
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice
{
    public class ThirdPartyInvoiceCaptureWizardConfiguration : HaloWizardBaseWorkflowConfiguration
    {
        public ThirdPartyInvoiceCaptureWizardConfiguration()
              : base("Third Party Invoice Capture", Shared.Configuration.WizardType.Sequential,
                   "Third Party Invoices", "Third Party Invoices", "Capture Invoice")
        {
        }

    }
}
