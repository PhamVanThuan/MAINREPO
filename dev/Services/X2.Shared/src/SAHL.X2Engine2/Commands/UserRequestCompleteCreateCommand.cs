using System.Collections.Generic;
using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Commands
{
    public class UserRequestCompleteCreateCommand : UserRequestCompleteActivityCommand
    {
        public UserRequestCompleteCreateCommand(long instanceID, Activity activity, string userName, bool ignoreWarnings, Dictionary<string, string> mapVariables = null, object data = null)
            : base(instanceID, activity, userName, ignoreWarnings, mapVariables, data)
        {
        }
    }
}