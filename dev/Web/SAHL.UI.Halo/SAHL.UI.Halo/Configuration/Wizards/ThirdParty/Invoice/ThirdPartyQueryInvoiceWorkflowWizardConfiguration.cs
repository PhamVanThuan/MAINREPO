using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice
{
    public class ThirdPartyQueryInvoiceWorkflowWizardConfiguration : HaloWizardBaseWorkflowConfiguration
    {
        public ThirdPartyQueryInvoiceWorkflowWizardConfiguration() 
            : base("Query on Invoice", Shared.Configuration.WizardType.Sequential, 
                   "Third Party Invoices", "Third Party Invoices", "Query on Invoice")
        {
        }
    }
}
