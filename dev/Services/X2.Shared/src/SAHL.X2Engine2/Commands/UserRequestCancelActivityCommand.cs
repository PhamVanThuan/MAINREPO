using SAHL.Core.Services;
using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Commands
{
    public class UserRequestCancelActivityCommand : ServiceCommand
    {
        public long InstanceId { get; set; }

        public string UserName { get; set; }

        public Activity Activity { get; set; }

        public UserRequestCancelActivityCommand(long instanceId, Activity activity, string userName)
        {
            this.InstanceId = instanceId;
            this.Activity = activity;
            this.UserName = userName;
        }
    }
}