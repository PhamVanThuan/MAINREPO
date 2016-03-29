
using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice
{
    public class ThirdPartyAcceptInvoiceWorkflowWizardPageConfiguration : HaloWizardTilePageBaseConfiguration,
                                                                          IHaloWizardTilePageConfiguration<ThirdPartyAcceptInvoiceWorkflowWizardConfiguration>,
                                                                          IHaloTilePageState<ThirdPartyAcceptInvoiceWorkflowWizardPageState>,
                                                                          IHaloTileModel<ThirdPartyInvoiceRootModel>
    {
        public ThirdPartyAcceptInvoiceWorkflowWizardPageConfiguration()
            : base("Accept Invoice", WizardPageType.Edit, sequence: 1)
        {
        }
    }
}
