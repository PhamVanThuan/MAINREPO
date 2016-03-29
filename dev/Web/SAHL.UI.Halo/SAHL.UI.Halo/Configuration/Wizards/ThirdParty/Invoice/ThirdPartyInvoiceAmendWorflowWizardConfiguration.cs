
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice
{
    public class ThirdPartyInvoiceAmendWorflowWizardConfiguration : HaloWizardBaseWorkflowConfiguration
    {
        public ThirdPartyInvoiceAmendWorflowWizardConfiguration()
            : base("Third Party Invoice Amend", Shared.Configuration.WizardType.Sequential,
                   "Third Party Invoices", "Third Party Invoices", "Amend Invoice")
        {
        }
    }
}
