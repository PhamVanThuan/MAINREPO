using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Third_Party_Invoices;
using SAHL.Core.SystemMessages;
using SAHL.Core.Data.Models.X2;
using SAHL.WorkflowMaps.Specs.Common;

namespace SAHL.WorkflowMaps.ThirdPartyInvoices.Specs
{
    public class WorkflowSpecThirdPartyInvoices : WorkflowSpec<X2Third_Party_Invoices, IX2Third_Party_Invoices_Data>
    {  
        public WorkflowSpecThirdPartyInvoices()
        {
            workflow = new X2Third_Party_Invoices();
            workflowData = new X2Third_Party_Invoices_Data();
        }
    }
}
