using System;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class InsertScheduledActivityCommand : ServiceCommand
    {
        public long InstanceId { get; protected set; }

        public DateTime TimeToExecute { get; set; }

        public ActivityDataModel ActivityDataModel { get; protected set; }

        public string WorkflowProviderName { get; protected set; }

        public InsertScheduledActivityCommand(long instanceId, DateTime timeToExecute, ActivityDataModel activityDataModel, string workflowProviderName)
        {
            this.InstanceId = instanceId;
            this.TimeToExecute = timeToExecute;
            this.ActivityDataModel = activityDataModel;
            this.WorkflowProviderName = workflowProviderName;
        }
    }
}