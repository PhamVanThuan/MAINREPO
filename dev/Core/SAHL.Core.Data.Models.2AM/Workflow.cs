using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class WorkflowDataModel :  IDataModel
    {
        public WorkflowDataModel(int workflowKey, int accountKey, int workflowStateKey, DateTime entryDate, string userName)
        {
            this.WorkflowKey = workflowKey;
            this.AccountKey = accountKey;
            this.WorkflowStateKey = workflowStateKey;
            this.EntryDate = entryDate;
            this.UserName = userName;
		
        }		

        public int WorkflowKey { get; set; }

        public int AccountKey { get; set; }

        public int WorkflowStateKey { get; set; }

        public DateTime EntryDate { get; set; }

        public string UserName { get; set; }
    }
}