using System.Collections.Generic;

namespace SAHL.Tools.Workflow.TestFolderGenerator
{
    public partial class OnEnter
    {
        private List<string> stateNames;
        private string workflowName;

        public OnEnter(IEnumerable<string> stateNames, string workflowName)
        {
            this.stateNames = new List<string>(stateNames);
            this.workflowName = workflowName;
        }

        public List<string> StateNames
        {
            get
            {
                return this.stateNames;
            }
        }

        public string WorkflowName
        {
            get
            {
                return this.workflowName;
            }
        }
    }
}