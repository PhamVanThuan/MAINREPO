
using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice
{
    public class ResolveThirdPartyInvoiceQueryWorkflowWizardPageConfiguration : HaloWizardTilePageBaseConfiguration,
                                                                          IHaloWizardTilePageConfiguration<ResolveThirdPartyInvoiceQueryWorkflowWizardConfiguration>,
                                                                          IHaloTilePageState<ResolveThirdPartyInvoiceQueryWorkflowWizardPageState>,
                                                                          IHaloTileModel<ThirdPartyInvoiceRootModel>
    {
        public ResolveThirdPartyInvoiceQueryWorkflowWizardPageConfiguration()
            : base("Query Resolved", WizardPageType.Edit, sequence: 1)
        {
        }
    }
}
