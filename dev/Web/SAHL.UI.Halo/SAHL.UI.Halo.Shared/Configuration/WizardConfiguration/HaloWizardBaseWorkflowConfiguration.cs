using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public abstract class HaloWizardBaseWorkflowConfiguration : HaloWizardBaseTileConfiguration, IHaloWizardWorkflowConfiguration
    {
        protected HaloWizardBaseWorkflowConfiguration(string name, WizardType wizardType,
                                                      string processName, string workflowName, string activityName) 
            : base(name, wizardType)
        {
            if (processName == null) { throw new ArgumentNullException("processName"); }
            if (workflowName == null) { throw new ArgumentNullException("workflowName"); }
            if (activityName == null) { throw new ArgumentNullException("activityName"); }

            this.ProcessName  = processName;
            this.WorkflowName = workflowName;
            this.ActivityName = activityName;
        }

        public string ProcessName { get; protected set; }
        public string WorkflowName { get; protected set; }
        public string ActivityName { get; protected set; }
    }
}
