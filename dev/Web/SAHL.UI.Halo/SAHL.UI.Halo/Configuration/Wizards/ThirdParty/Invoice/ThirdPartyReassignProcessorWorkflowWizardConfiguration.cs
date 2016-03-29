using SAHL.UI.Halo.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice
{
    public class ThirdPartyReassignProcessorWorkflowWizardConfiguration : HaloWizardBaseWorkflowConfiguration
    {
        public ThirdPartyReassignProcessorWorkflowWizardConfiguration() :
            base("Third Party Reassign Processor", Shared.Configuration.WizardType.Sequential,
                    "Third Party Invoices", "Third Party Invoices", "Reassign Processor")
        {
        }
    }
}
