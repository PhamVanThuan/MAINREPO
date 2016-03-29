using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Commands
{
    public class QueueUpExternalActivitiesCommand : ServiceCommand
    {
        public InstanceDataModel Instance { get; protected set; }

        public Activity Activity { get; protected set; }

        public string UserName { get; set; }

        public bool IgnoreWarnings { get; set; }

        public object Data { get; set; }

        public QueueUpExternalActivitiesCommand(InstanceDataModel instance, Activity activity)
        {
            this.Instance = instance;
            this.Activity = activity;
            this.UserName = "X2";
            this.IgnoreWarnings = true;
        }
    }
}