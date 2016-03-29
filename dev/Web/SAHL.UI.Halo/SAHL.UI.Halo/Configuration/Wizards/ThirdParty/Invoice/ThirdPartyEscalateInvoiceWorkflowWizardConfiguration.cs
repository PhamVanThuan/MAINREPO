

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice
{
    public class ThirdPartyEscalateInvoiceWorkflowWizardConfiguration : HaloWizardBaseWorkflowConfiguration
    {
        public ThirdPartyEscalateInvoiceWorkflowWizardConfiguration() 
            : base("Escalate for Approval", Shared.Configuration.WizardType.Sequential, 
                   "Third Party Invoices", "Third Party Invoices", "Escalate for Approval")
        {
        }
    }
}
