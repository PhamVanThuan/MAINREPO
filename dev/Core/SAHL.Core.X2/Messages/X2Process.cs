using System.Collections.Generic;

namespace SAHL.Core.X2.Messages
{
    public class X2Process
    {
        public X2Process(string processName, IEnumerable<X2Workflow> workflows)
        {
            this.ProcessName = processName;
            if (workflows != null)
            {
                this.Workflows = new List<X2Workflow>(workflows);
            }
        }

        public string ProcessName { get; protected set; }

        public IEnumerable<X2Workflow> Workflows
        {
            get;
            protected set;
        }
    }
}