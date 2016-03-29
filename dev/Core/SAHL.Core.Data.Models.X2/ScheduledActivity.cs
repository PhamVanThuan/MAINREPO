using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class ScheduledActivityDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ScheduledActivityDataModel(long instanceID, DateTime time, int activityID, int priority, string workFlowProviderName)
        {
            this.InstanceID = instanceID;
            this.Time = time;
            this.ActivityID = activityID;
            this.Priority = priority;
            this.WorkFlowProviderName = workFlowProviderName;
		
        }
		[JsonConstructor]
        public ScheduledActivityDataModel(long instanceID, DateTime time, int activityID, int priority, string workFlowProviderName, int iD)
        {
            this.InstanceID = instanceID;
            this.Time = time;
            this.ActivityID = activityID;
            this.Priority = priority;
            this.WorkFlowProviderName = workFlowProviderName;
            this.ID = iD;
		
        }		

        public long InstanceID { get; set; }

        public DateTime Time { get; set; }

        public int ActivityID { get; set; }

        public int Priority { get; set; }

        public string WorkFlowProviderName { get; set; }

        public int ID { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}