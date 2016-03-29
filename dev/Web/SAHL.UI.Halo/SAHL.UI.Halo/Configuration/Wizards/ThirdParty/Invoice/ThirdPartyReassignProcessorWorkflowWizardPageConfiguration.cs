using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice
{
    public class ThirdPartyReassignProcessorWorkflowWizardPageConfiguration : HaloWizardTilePageBaseConfiguration,
                                                                          IHaloWizardTilePageConfiguration<ThirdPartyReassignProcessorWorkflowWizardConfiguration>,
                                                                          IHaloTilePageState<ThirdPartyReassignProcessorWorkflowWizardPageState>,
                                                                          IHaloTileModel<ThirdPartyInvoiceRootModel>
    {
        public ThirdPartyReassignProcessorWorkflowWizardPageConfiguration()
            : base("Reassign Processor", WizardPageType.Edit, sequence: 1)
        {
        }
    }
}
