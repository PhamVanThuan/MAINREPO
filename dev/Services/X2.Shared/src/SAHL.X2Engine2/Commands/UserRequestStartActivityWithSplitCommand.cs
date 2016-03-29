using System.Collections.Generic;
using SAHL.Core.Services;
using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Commands
{
    public class UserRequestStartActivityWithSplitCommand : ServiceCommand, ISplittable, ICanIgnoreWarningsCommand, IIgnoreWarnings, IContinueWithCommands
    {
        public long InstanceId { get; set; }

        public string Username { get; set; }

        public Activity Activity { get; set; }

        public Dictionary<string, string> MapVariables { get; set; }

        public string WorkflowProviderName { get; protected set; }

        public bool IgnoreWarnings { get; set; }

        public object Data { get; set; }

        public UserRequestStartActivityWithSplitCommand(long instanceID, string username, Activity activity, Dictionary<string, string> mapVariables, string workflowProviderName, bool ignoreWarnings, object data)
        {
            this.InstanceId = instanceID;
            this.Username = username;
            this.Activity = activity;
            this.MapVariables = mapVariables;
            this.WorkflowProviderName = workflowProviderName;
            this.IgnoreWarnings = ignoreWarnings;
            this.Data = data;
        }
    }
}