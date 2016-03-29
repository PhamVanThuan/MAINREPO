using System.Collections.Generic;

namespace SAHL.Tools.Workflow.Common.Database.Publishing
{
    public class PublisherOptions
    {
        public PublisherOptions(IEnumerable<ProcessOption> processOptions, string binaryDirectory, TargetEnvironment targetEnvironment)
        {
            this.BinaryDirectory = binaryDirectory;
            this.ProcessesToPublish = new List<ProcessOption>(processOptions);
            this.TargetEnvironment = targetEnvironment;
        }

        public string BinaryDirectory { get; protected set; }

        public List<ProcessOption> ProcessesToPublish { get; protected set; }

        public TargetEnvironment TargetEnvironment { get; protected set; }
    }
}