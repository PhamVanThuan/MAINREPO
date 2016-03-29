using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice
{
    public class ThirdPartyQueryInvoiceWorkflowWizardPageConfiguration : HaloWizardTilePageBaseConfiguration,
                                                                          IHaloWizardTilePageConfiguration<ThirdPartyQueryInvoiceWorkflowWizardConfiguration>,
                                                                          IHaloTilePageState<ThirdPartyQueryInvoiceWorkflowWizardPageState>,
                                                                          IHaloTileModel<ThirdPartyInvoiceRootModel>
    {
        public ThirdPartyQueryInvoiceWorkflowWizardPageConfiguration()
            : base("Query on Invoice", WizardPageType.Edit, sequence: 1)
        {
        }
    }
}
