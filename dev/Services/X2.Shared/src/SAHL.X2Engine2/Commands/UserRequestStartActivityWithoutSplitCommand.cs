using System.Collections.Generic;
using SAHL.Core.Services;
using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Commands
{
    public class UserRequestStartActivityWithoutSplitCommand : ServiceCommand, ICanIgnoreWarningsCommand, IIgnoreWarnings, IContinueWithCommands
    {
        public long InstanceId { get; set; }

        public string Username { get; set; }

        public Activity Activity { get; set; }

        public bool IgnoreWarnings { get; set; }

        public Dictionary<string, string> MapVariables { get; set; }

        public object Data { get; set; }

        public UserRequestStartActivityWithoutSplitCommand(long instanceID, string username, Activity activity, bool ignoreWarnings, Dictionary<string, string> mapVariables, object data = null)
        {
            this.InstanceId = instanceID;
            this.Username = username;
            this.Activity = activity;
            this.MapVariables = mapVariables;
            this.IgnoreWarnings = ignoreWarnings;
            this.Data = data;
        }
    }
}