using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class CreateInstanceCommand : ServiceCommand
    {
        public string ProcessName { get; protected set; }

        public string WorkflowName { get; protected set; }

        public long? SourceInstanceId { get; set; }

        public int? ReturnActivityId { get; set; }

        public long NewlyCreatedInstanceId { get; set; }

        public string ActivityName { get; protected set; }

        public string UserName { get; protected set; }

        public string WorkflowProviderName { get; protected set; }

        public long? ParentInstanceID { get; set; }

        public CreateInstanceCommand(string processName, string workflowName, string activityName, string userName, string workflowProviderName, long? parentInstanceId, long? sourceInstanceID = null, int? returnActivityId = null)
        {
            this.ProcessName = processName;
            this.WorkflowName = workflowName;
            this.ActivityName = activityName;
            this.UserName = userName;
            this.WorkflowProviderName = workflowProviderName;
            this.SourceInstanceId = sourceInstanceID;
            this.ReturnActivityId = returnActivityId;
            this.ParentInstanceID = parentInstanceId;
        }
    }
}