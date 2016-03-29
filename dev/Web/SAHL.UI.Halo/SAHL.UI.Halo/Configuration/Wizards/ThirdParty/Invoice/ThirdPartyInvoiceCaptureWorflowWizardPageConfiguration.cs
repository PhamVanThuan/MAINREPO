
using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice
{
    public class ThirdPartyInvoiceCaptureWizardPageConfiguration : HaloWizardTilePageBaseConfiguration,
                                                                    IHaloWizardTilePageConfiguration<ThirdPartyInvoiceCaptureWizardConfiguration>,
                                                                    IHaloTilePageState<ThirdPartyInvoiceCaptureWizardPageState>,
                                                                    IHaloTileModel<ThirdPartyInvoiceRootModel>
    {
        public ThirdPartyInvoiceCaptureWizardPageConfiguration()
            : base("Invoice Capture", WizardPageType.Edit, sequence: 1)
        {
        }
    }
}
