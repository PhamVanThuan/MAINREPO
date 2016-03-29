using System.Collections.Generic;
using SAHL.Core.Services;
using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Commands
{
    public class UserRequestCompleteActivityCommand : ServiceCommand, IContinueWithCommands, ICanIgnoreWarningsCommand, IIgnoreWarnings
    {
        public long InstanceId { get; set; }

        public string UserName { get; set; }

        public Activity Activity { get; set; }

        public Dictionary<string, string> MapVariables { get; set; }

        public bool IgnoreWarnings { get; set; }

        public object Data { get; set; }

        public UserRequestCompleteActivityCommand(long instanceID, Activity activity, string userName, bool ignoreWarnings, Dictionary<string, string> mapVariables = null, object data = null)
        {
            this.InstanceId = instanceID;
            this.UserName = userName;
            this.Activity = activity;
            this.MapVariables = mapVariables;
            this.IgnoreWarnings = ignoreWarnings;
            this.Data = data;
        }
    }
}