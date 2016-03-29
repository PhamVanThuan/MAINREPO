using System.Collections.Generic;

namespace SAHL.Tools.Workflow.Common.Database.Publishing
{
    public class ProcessOption
    {
        public ProcessOption(string pathToProcessFile)
        {
            this.PathToProcessFile = pathToProcessFile;
        }

        public string PathToProcessFile { get; protected set; }

        public List<WorkflowOption> WorkflowOptions { get; protected set; }
    }
}