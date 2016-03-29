using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.SystemMessages;
using SAHL.Core.Data.Models.X2;
using SAHL.WorkflowMaps.Specs.Common;
using LifeClaims;

namespace SAHL.WorkflowMaps.LifeClaims.Specs
{
    public class WorkflowSpecDisabilityClaim : WorkflowSpec<X2Disability_Claim, IX2Disability_Claim_Data>
    {
        public WorkflowSpecDisabilityClaim()
        {
            workflow = new X2Disability_Claim();
            workflowData = new X2Disability_Claim_Data();
        }
    }
}
