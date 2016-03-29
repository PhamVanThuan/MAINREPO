using System;

namespace SAHL.Services.Interfaces.FrontEndTest.Models
{
    public class GetThirdPartyInvoicesInstanceDetailsQueryResult
    {
        public GetThirdPartyInvoicesInstanceDetailsQueryResult(long instanceID,
            int workFlowID,
            long parentInstanceID,
            string name,
            string subject,
            string workFlowProvider,
            int stateID,
            string creatorADUserName,
            DateTime creationDate,
            DateTime stateChangeDate,
            DateTime deadlineDate,
            DateTime activityDate,
            string activityADUserName,
            int activityID,
            int priority,
            long sourceInstanceID,
            int returnActivityID,
            int processID,
            int workFlowAncestorID,
            string workFlowName,
            DateTime createDate,
            string storageTable,
            string storageKey,
            int iconID,
            string defaultSubject,
            int genericKeyTypeKey,
            int stateWorkFlowID,
            string stateName,
            int type,
            bool forwardState,
            int sequence,
            int returnWorkflowID,
            int stateReturnActivityID,
            int thirdPartyInvoiceKey,
            int accountKey,
            int thirdPartyTypeKey,
            int genericKey)
        {
            this.InstanceID = instanceID;
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
            this.ProcessID = processID;
            this.WorkFlowAncestorID = workFlowAncestorID;
            this.WorkFlowName = workFlowName;
            this.CreateDate = createDate;
            this.StorageTable = storageTable;
            this.StorageKey = storageKey;
            this.IconID = iconID;
            this.DefaultSubject = defaultSubject;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.StateWorkFlowID = stateWorkFlowID;
            this.StateName = stateName;
            this.Type = type;
            this.ForwardState = forwardState;
            this.Sequence = sequence;
            this.ReturnWorkflowID = returnWorkflowID;
            this.StateReturnActivityID = stateReturnActivityID;
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.AccountKey = accountKey;
            this.ThirdPartyTypeKey = thirdPartyTypeKey;
            this.GenericKey = genericKey;
        }

        public long InstanceID { get; set; }

        public int WorkFlowID { get; set; }

        public long ParentInstanceID { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        public string WorkFlowProvider { get; set; }

        public int StateID { get; set; }

        public string CreatorADUserName { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime StateChangeDate { get; set; }

        public DateTime DeadlineDate { get; set; }

        public DateTime ActivityDate { get; set; }

        public string ActivityADUserName { get; set; }

        public int ActivityID { get; set; }

        public int Priority { get; set; }

        public long SourceInstanceID { get; set; }

        public int ReturnActivityID { get; set; }

        public int ProcessID { get; set; }

        public int WorkFlowAncestorID { get; set; }

        public string WorkFlowName { get; set; }

        public DateTime CreateDate { get; set; }

        public string StorageTable { get; set; }

        public string StorageKey { get; set; }

        public int IconID { get; set; }

        public string DefaultSubject { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public int StateWorkFlowID { get; set; }

        public string StateName { get; set; }

        public int Type { get; set; }

        public bool ForwardState { get; set; }

        public int Sequence { get; set; }

        public int ReturnWorkflowID { get; set; }

        public int StateReturnActivityID { get; set; }

        public int ThirdPartyInvoiceKey { get; set; }

        public int AccountKey { get; set; }

        public int ThirdPartyTypeKey { get; set; }

        public int GenericKey { get; set; }
    }
}