using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public class HaloWorkflowAction : IHaloWorkflowAction
    {
        public HaloWorkflowAction(string name, string iconName, string iconGroup, int sequence, 
                                  string processName, string workflowName, long instanceId)
        {
            this.Name         = name;
            this.IconName     = iconName;
            this.Group        = iconGroup;
            this.Sequence     = sequence;
            this.ProcessName  = processName;
            this.WorkflowName = workflowName;
            this.InstanceId   = instanceId;
        }

        public string Name { get; protected set; }
        public string IconName { get; protected set; }
        public string Group { get; protected set; }
        public int Sequence { get; protected set; }

        public string ProcessName { get; protected set; }
        public string WorkflowName { get; protected set; }
        public long InstanceId { get; protected set; }
    }
}
