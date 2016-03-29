using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice
{
    public class ThirdPartyAcceptInvoiceWorkflowWizardConfiguration : HaloWizardBaseWorkflowConfiguration
    {
        public ThirdPartyAcceptInvoiceWorkflowWizardConfiguration() 
            : base("Third Party Accept Invoice", Shared.Configuration.WizardType.Sequential, 
                   "Third Party Invoices", "Third Party Invoices", "Accept Invoice")
        {
        }
    }
}
