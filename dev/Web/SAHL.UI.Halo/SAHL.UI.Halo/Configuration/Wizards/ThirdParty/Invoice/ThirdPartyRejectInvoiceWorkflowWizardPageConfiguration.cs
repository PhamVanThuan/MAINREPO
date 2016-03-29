
using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice
{
    public class ThirdPartyRejectInvoiceWorkflowWizardPageConfiguration : HaloWizardTilePageBaseConfiguration,
                                                                          IHaloWizardTilePageConfiguration<ThirdPartyRejectInvoiceWorkflowWizardConfiguration>,
                                                                          IHaloTilePageState<ThirdPartyRejectInvoiceWorkflowWizardPageState>,
                                                                          IHaloTileModel<ThirdPartyInvoiceRootModel>
    {
        public ThirdPartyRejectInvoiceWorkflowWizardPageConfiguration()
            : base("Reject Invoice", WizardPageType.Edit, sequence: 1)
        {
        }
    }
}
