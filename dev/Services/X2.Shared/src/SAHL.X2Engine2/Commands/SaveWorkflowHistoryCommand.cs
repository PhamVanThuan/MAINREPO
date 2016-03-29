using System;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class SaveWorkflowHistoryCommand : ServiceCommand
    {
        public InstanceDataModel Instance { get; protected set; }

        public int ToStateID { get; protected set; }

        public int ActivityID { get; protected set; }

        public string UserWhoPerformedTheActivity { get; protected set; }

        public DateTime ActivityTime { get; protected set; }

        public int WorkflowHistoryId { get; set; }

        public SaveWorkflowHistoryCommand(InstanceDataModel instance, int toStateID, int activityID, string userWhoPerformedTheActivity, DateTime activityTime)
        {
            this.Instance = instance;
            this.ToStateID = toStateID;
            this.ActivityID = activityID;
            this.UserWhoPerformedTheActivity = userWhoPerformedTheActivity;
            this.ActivityTime = activityTime;
        }
    }
}