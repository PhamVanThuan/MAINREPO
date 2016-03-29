
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice
{
    public class ThirdPartyApproveInvoiceWorkflowWizardConfiguration : HaloWizardBaseWorkflowConfiguration
    {
        public ThirdPartyApproveInvoiceWorkflowWizardConfiguration() 
            : base("Third Party Approve Invoice", Shared.Configuration.WizardType.Sequential, 
                   "Third Party Invoices", "Third Party Invoices", "Approve for Payment")
        {
        }
    }
}