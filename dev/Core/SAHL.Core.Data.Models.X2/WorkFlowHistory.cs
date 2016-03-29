using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class WorkFlowHistoryDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public WorkFlowHistoryDataModel(long instanceID, int stateID, int? activityID, string creatorADUserName, DateTime creationDate, DateTime stateChangeDate, DateTime? deadlineDate, DateTime activityDate, string aDUserName, int? priority)
        {
            this.InstanceID = instanceID;
            this.StateID = stateID;
            this.ActivityID = activityID;
            this.CreatorADUserName = creatorADUserName;
            this.CreationDate = creationDate;
            this.StateChangeDate = stateChangeDate;
            this.DeadlineDate = deadlineDate;
            this.ActivityDate = activityDate;
            this.ADUserName = aDUserName;
            this.Priority = priority;
		
        }
		[JsonConstructor]
        public WorkFlowHistoryDataModel(int iD, long instanceID, int stateID, int? activityID, string creatorADUserName, DateTime creationDate, DateTime stateChangeDate, DateTime? deadlineDate, DateTime activityDate, string aDUserName, int? priority)
        {
            this.ID = iD;
            this.InstanceID = instanceID;
            this.StateID = stateID;
            this.ActivityID = activityID;
            this.CreatorADUserName = creatorADUserName;
            this.CreationDate = creationDate;
            this.StateChangeDate = stateChangeDate;
            this.DeadlineDate = deadlineDate;
            this.ActivityDate = activityDate;
            this.ADUserName = aDUserName;
            this.Priority = priority;
		
        }		

        public int ID { get; set; }

        public long InstanceID { get; set; }

        public int StateID { get; set; }

        public int? ActivityID { get; set; }

        public string CreatorADUserName { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime StateChangeDate { get; set; }

        public DateTime? DeadlineDate { get; set; }

        public DateTime ActivityDate { get; set; }

        public string ADUserName { get; set; }

        public int? Priority { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}