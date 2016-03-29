
using SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.Actions
{
    public class ThirdPartyInvoiceAmendWizardAction : HaloTileWizardActionBase<ThirdPartyInvoiceRootTileConfiguration, ThirdPartyInvoiceAmendWorflowWizardConfiguration>
    {
        public ThirdPartyInvoiceAmendWizardAction(string contextData = null)
            : base("Amend Invoice", "icon-plus-2", "Capture", 1, contextData)
        {
        }
    }
}
