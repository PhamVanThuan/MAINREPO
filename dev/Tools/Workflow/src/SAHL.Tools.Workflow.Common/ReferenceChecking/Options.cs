namespace SAHL.Tools.Workflow.Common.ReferenceChecking
{
    public class Options
    {
		public Options(string workflowMapPath, string rootPath, string binariesPath)
        {
            this.WorkflowMapPath = workflowMapPath;
            this.RootPath = rootPath;
			this.BinariesPath = binariesPath;
        }

        public string WorkflowMapPath { get; protected set; }

        public string RootPath { get; protected set; }

        public string BinariesPath { get; protected set; }
    }
}