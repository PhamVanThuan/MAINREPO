using System.Collections.Generic;

namespace SAHL.X2Engine2.Node.Providers
{
    public class X2ProcessConfiguration
    {
        public X2ProcessConfiguration(string processName, IEnumerable<X2WorkflowConfiguration> workflows)
        {
            this.ProcessName = processName;
            if (workflows != null)
            {
                this.WorkflowConfigurations = new List<X2WorkflowConfiguration>(workflows);
            }
        }

        public string ProcessName { get; protected set; }

        public IEnumerable<X2WorkflowConfiguration> WorkflowConfigurations
        {
            get;
            protected set;
        }
    }
}