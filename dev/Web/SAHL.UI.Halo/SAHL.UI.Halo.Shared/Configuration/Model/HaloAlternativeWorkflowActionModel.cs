using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SAHL.Core.BusinessModel;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public class HaloAlternativeWorkflowActionModel
    {
        public HaloAlternativeWorkflowActionModel(string processName, string workflowName, string activityName)
        {
            if (string.IsNullOrWhiteSpace(processName)) { throw new ArgumentNullException("processName"); }
            if (string.IsNullOrWhiteSpace(workflowName)) { throw new ArgumentNullException("workflowName"); }
            if (string.IsNullOrWhiteSpace(activityName)) { throw new ArgumentNullException("activityName"); }

            this.ProcessName  = processName;
            this.WorkflowName = workflowName;
            this.ActivityName = activityName;
        }

        public string ProcessName { get; set; }
        public string WorkflowName { get; set; }
        public string ActivityName { get; set; }
    }
}
