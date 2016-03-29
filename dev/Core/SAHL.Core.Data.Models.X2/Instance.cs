using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class InstanceDataModel : MarshalByRefObject, IDataModel, IDataModelWithIdentitySeed
    {
        public InstanceDataModel(int workFlowID, long? parentInstanceID, string name, string subject, string workFlowProvider, int? stateID, string creatorADUserName, DateTime creationDate, DateTime? stateChangeDate, DateTime? deadlineDate, DateTime? activityDate, string activityADUserName, int? activityID, int? priority, long? sourceInstanceID, int? returnActivityID)
        {
            this.WorkFlowID = workFlowID;
            this.ParentInstanceID = parentInstanceID;
            this.Name = name;
            this.Subject = subject;
            this.WorkFlowProvider = workFlowProvider;
            this.StateID = stateID;
            this.CreatorADUserName = creatorADUserName;
            this.CreationDate = creationDate;
            this.StateChangeDate = stateChangeDate;
            this.DeadlineDate = deadlineDate;
            this.ActivityDate = activityDate;
            this.ActivityADUserName = activityADUserName;
            this.ActivityID = activityID;
            this.Priority = priority;
            this.SourceInstanceID = sourceInstanceID;
            this.ReturnActivityID = returnActivityID;
		
        }
		[JsonConstructor]
        public InstanceDataModel(long iD, int workFlowID, long? parentInstanceID, string name, string subject, string workFlowProvider, int? stateID, string creatorADUserName, DateTime creationDate, DateTime? stateChangeDate, DateTime? deadlineDate, DateTime? activityDate, string activityADUserName, int? activityID, int? priority, long? sourceInstanceID, int? returnActivityID)
        {
            this.ID = iD;
            this.WorkFlowID = workFlowID;
            this.ParentInstanceID = parentInstanceID;
            this.Name = name;
            this.Subject = subject;
            this.WorkFlowProvider = workFlowProvider;
            this.StateID = stateID;
            this.CreatorADUserName = creatorADUserName;
            this.CreationDate = creationDate;
            this.StateChangeDate = stateChangeDate;
            this.DeadlineDate = deadlineDate;
            this.ActivityDate = activityDate;
            this.ActivityADUserName = activityADUserName;
            this.ActivityID = activityID;
            this.Priority = priority;
            this.SourceInstanceID = sourceInstanceID;
            this.ReturnActivityID = returnActivityID;
		
        }		

        public long ID { get; set; }

        public int WorkFlowID { get; set; }

        public long? ParentInstanceID { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        public string WorkFlowProvider { get; set; }

        public int? StateID { get; set; }

        public string CreatorADUserName { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? StateChangeDate { get; set; }

        public DateTime? DeadlineDate { get; set; }

        public DateTime? ActivityDate { get; set; }

        public string ActivityADUserName { get; set; }

        public int? ActivityID { get; set; }

        public int? Priority { get; set; }

        public long? SourceInstanceID { get; set; }

        public int? ReturnActivityID { get; set; }

        public void SetKey(long key)
        {
            this.ID =  key;
        }
    }
}