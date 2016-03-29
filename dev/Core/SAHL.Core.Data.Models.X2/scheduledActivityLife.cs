using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class ScheduledActivityLifeDataModel :  IDataModel
    {
        public ScheduledActivityLifeDataModel(long? instanceID, DateTime? time, int? activityID, int? priority, string workflowProviderName)
        {
            this.InstanceID = instanceID;
            this.Time = time;
            this.ActivityID = activityID;
            this.Priority = priority;
            this.WorkflowProviderName = workflowProviderName;
		
        }		

        public long? InstanceID { get; set; }

        public DateTime? Time { get; set; }

        public int? ActivityID { get; set; }

        public int? Priority { get; set; }

        public string WorkflowProviderName { get; set; }
    }
}