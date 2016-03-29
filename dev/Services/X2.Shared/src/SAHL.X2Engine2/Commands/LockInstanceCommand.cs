using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Commands
{
    public class LockInstanceCommand : ServiceCommand
    {
        //public long InstanceID { get; protected set; }
        public InstanceDataModel Instance { get; protected set; }

        public string Username { get; protected set; }

        public Activity Activity { get; protected set; }

        //public LockInstanceCommand(long instanceID, string username, Activity activity)
        public LockInstanceCommand(InstanceDataModel instance, string username, Activity activity)
        {
            this.Instance = instance;
            this.Username = username;
            this.Activity = activity;
        }
    }
}