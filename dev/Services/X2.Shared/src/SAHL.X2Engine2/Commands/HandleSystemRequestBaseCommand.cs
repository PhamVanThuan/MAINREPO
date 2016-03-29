using SAHL.Core.Services;
using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Commands
{
    public class HandleSystemRequestBaseCommand : ServiceCommand, ICanIgnoreWarningsCommand
    {
        public long InstanceId { get; protected set; }

        public Activity Activity { get; protected set; }

        public string UserName { get; set; }

        public bool Result { get; set; }

        public bool IgnoreWarnings { get; set; }

        public object Data { get; set; }

        public HandleSystemRequestBaseCommand(long instanceId, Activity activity, string userName, object data = null)
        {
            this.InstanceId = instanceId;
            this.Activity = activity;
            this.UserName = userName;
            this.IgnoreWarnings = false;
            this.Data = data;
        }
    }
}