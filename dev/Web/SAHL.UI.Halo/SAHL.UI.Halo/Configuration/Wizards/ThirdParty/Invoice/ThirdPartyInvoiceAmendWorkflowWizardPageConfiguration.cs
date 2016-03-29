
using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice
{
    public class ThirdPartyInvoiceAmendWorkflowWizardPageConfiguration : HaloWizardTilePageBaseConfiguration,
                                                                    IHaloWizardTilePageConfiguration<ThirdPartyInvoiceAmendWorflowWizardConfiguration>,
                                                                    IHaloTilePageState<ThirdPartyInvoiceAmendWorflowWizardPageState>,
                                                                    IHaloTileModel<ThirdPartyInvoiceRootModel>
    {
        public ThirdPartyInvoiceAmendWorkflowWizardPageConfiguration()
            : base("Amend Invoice", WizardPageType.Edit, sequence: 1)
        {
        }
    }
}
