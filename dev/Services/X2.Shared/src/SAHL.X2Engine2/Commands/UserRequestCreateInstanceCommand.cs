using System.Collections.Generic;
using SAHL.Core.Services;
using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Commands
{
    public class UserRequestCreateInstanceCommand : ServiceCommand, IIgnoreWarnings
    {
        public string ProcessName { get; set; }

        public string WorkflowName { get; set; }

        public string Username { get; set; }

        public Activity Activity { get; set; }

        public Dictionary<string, string> MapVariables { get; set; }

        public string WorkflowProviderName { get; set; }

        public bool IgnoreWarnings { get; set; }

        public long NewlyCreatedInstanceId { get; set; }

        public long? ParentInstanceId { get; set; }

        public long? SourceInstanceId { get; set; }

        public int? ReturnActivityId { get; set; }

        public object Data { get; set; }

        public UserRequestCreateInstanceCommand(string processName, string workflowName, string username, Activity activity, Dictionary<string, string> mapVariables, string workflowProviderName, bool ignoreWarnings, long? parentInstanceId = null, long? sourceInstanceId = null, int? returnActivityId = null, object data = null)
        {
            this.ProcessName = processName;
            this.WorkflowName = workflowName;
            this.Username = username;
            this.Activity = activity;
            this.MapVariables = mapVariables;
            this.WorkflowProviderName = workflowProviderName;
            this.ParentInstanceId = parentInstanceId;
            this.SourceInstanceId = sourceInstanceId;
            this.ReturnActivityId = returnActivityId;
            this.IgnoreWarnings = ignoreWarnings;
            this.Data = data;
        }
    }
}