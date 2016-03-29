using System.Collections.Generic;

namespace Automation.Framework
{
    public class WorkflowScript
    {
        public List<Script> Scripts { get; set; }

        public string DataTable { get; set; }

        public string WorkflowName { get; set; }

        public string PrimaryKey { get; set; }

        public WorkflowScript()
        {
            Scripts = new List<Script>();
        }

        public string ProcessName { get; set; }
    }
}