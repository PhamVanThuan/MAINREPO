using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice
{
    public class ThirdPartyEscalateInvoiceWorkflowWizardPageConfiguration : HaloWizardTilePageBaseConfiguration,
                                                                            IHaloWizardTilePageConfiguration<ThirdPartyEscalateInvoiceWorkflowWizardConfiguration>,
                                                                            IHaloTilePageState<ThirdPartyEscalateInvoiceWorkflowWizardPageState>,
                                                                            IHaloTileModel<ThirdPartyInvoiceRootModel>
    {
        public ThirdPartyEscalateInvoiceWorkflowWizardPageConfiguration()
            : base("Escalate for Approval", WizardPageType.Edit, sequence: 1)
        {
        }
    }
}
