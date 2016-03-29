using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.Services;
using SAHL.Core.BusinessModel;
using SAHL.Services.Interfaces.Halo.Models;

namespace SAHL.Services.Interfaces.Halo.Queries
{
    public class GetWizardConfigurationQuery : ServiceQuery<WizardConfigurationQueryResult>, IHaloServiceQuery
    {
        public GetWizardConfigurationQuery(string wizardName, string processName, string workflowName, string activityName, BusinessContext businessContext)
        {
            if (businessContext == null) { throw new ArgumentNullException("businessContext"); }

            this.WizardName      = wizardName;
            this.WorkflowName    = workflowName;
            this.ActivityName    = activityName;
            this.ProcessName     = processName;
            this.BusinessContext = businessContext;
        }

        public string WizardName { get; protected set; }
        public string WorkflowName { get; protected set; }
        public string ProcessName { get; protected set; }
        public string ActivityName { get; protected set; }

        public BusinessContext BusinessContext { get; protected set; }
    }
}
