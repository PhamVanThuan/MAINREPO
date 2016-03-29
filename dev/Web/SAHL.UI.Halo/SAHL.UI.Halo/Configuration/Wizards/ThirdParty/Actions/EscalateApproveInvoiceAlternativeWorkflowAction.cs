using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Actions
{
    public class EscalateApproveInvoiceAlternativeWorkflowAction : HaloWorkflowAction
    {
        public EscalateApproveInvoiceAlternativeWorkflowAction() 
            : base("Escalate for Approval", "workflow-instance", "Workflow", 1, "Third Party Invoices", "Third Party Invoices", 1)
        {
        }
    }
}
