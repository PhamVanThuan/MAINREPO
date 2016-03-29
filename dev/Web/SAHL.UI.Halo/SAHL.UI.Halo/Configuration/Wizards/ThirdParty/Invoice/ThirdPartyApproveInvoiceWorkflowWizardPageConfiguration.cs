using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice
{
    public class ThirdPartyApproveInvoiceWorkflowWizardPageConfiguration : HaloWizardTilePageBaseConfiguration,
                                                                          IHaloWizardTilePageConfiguration<ThirdPartyApproveInvoiceWorkflowWizardConfiguration>,
                                                                          IHaloTilePageState<ThirdPartyApproveInvoiceWorkflowWizardPageState>,
                                                                          IHaloTileModel<ThirdPartyInvoiceRootModel>
    {
        public ThirdPartyApproveInvoiceWorkflowWizardPageConfiguration()
            : base("Approve for Payment", WizardPageType.Edit, sequence: 1)
        {
        }
    }
}
