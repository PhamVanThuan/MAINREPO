
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice
{
    public class ThirdPartyRejectInvoiceWorkflowWizardConfiguration : HaloWizardBaseWorkflowConfiguration
    {
         public ThirdPartyRejectInvoiceWorkflowWizardConfiguration() 
            : base("Third Party Reject Invoice", Shared.Configuration.WizardType.Sequential, 
                   "Third Party Invoices", "Third Party Invoices", "Reject Invoice")
        {
        }
    }
}
