using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SAHL.UI.Halo.Shared.Configuration
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class HaloTileLinkedAttribute : Attribute
    {
        public HaloTileLinkedAttribute(string processName, string workflowName)
        {
            if (string.IsNullOrWhiteSpace(processName)) { throw new ArgumentNullException("processName"); }
            if (string.IsNullOrWhiteSpace(workflowName)) { throw new ArgumentNullException("workflowName"); }

            this.ProcessName = processName;
            this.WorkflowName = workflowName;
        }

        public string Name
        {
            get { return string.Format("{0} - {1}", this.ProcessName, this.WorkflowName); }
        }

        public string ProcessName { get; private set; }
        public string WorkflowName { get; private set; }
    }
}
