using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class InstanceArchiveDataModel :  IDataModel
    {
        public InstanceArchiveDataModel(int workFlowID, long? parentInstanceID, string name, string subject, string workFlowProvider, int? stateID, string creatorADUserName, DateTime creationDate, DateTime? stateChangeDate, DateTime? deadlineDate, int? priority, long? sourceInstanceID, int? returnActivityID, DateTime archiveDate)
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
            this.Priority = priority;
            this.SourceInstanceID = sourceInstanceID;
            this.ReturnActivityID = returnActivityID;
            this.ArchiveDate = archiveDate;
		
        }
		[JsonConstructor]
        public InstanceArchiveDataModel(long iD, int workFlowID, long? parentInstanceID, string name, string subject, string workFlowProvider, int? stateID, string creatorADUserName, DateTime creationDate, DateTime? stateChangeDate, DateTime? deadlineDate, int? priority, long? sourceInstanceID, int? returnActivityID, DateTime archiveDate)
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
            this.Priority = priority;
            this.SourceInstanceID = sourceInstanceID;
            this.ReturnActivityID = returnActivityID;
            this.ArchiveDate = archiveDate;
		
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

        public int? Priority { get; set; }

        public long? SourceInstanceID { get; set; }

        public int? ReturnActivityID { get; set; }

        public DateTime ArchiveDate { get; set; }
    }
}