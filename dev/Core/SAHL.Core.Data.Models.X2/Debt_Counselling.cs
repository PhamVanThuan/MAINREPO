using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class Debt_CounsellingDataModel :  IDataModel
    {
        public Debt_CounsellingDataModel(long instanceID, int? debtCounsellingKey, int? accountKey, bool? sentToLitigation, string assignADUserName, int? assignWorkflowRoleTypeKey, string previousState, bool? courtCase, int? latestReasonDefinitionKey, int productKey, int genericKey)
        {
            this.InstanceID = instanceID;
            this.DebtCounsellingKey = debtCounsellingKey;
            this.AccountKey = accountKey;
            this.SentToLitigation = sentToLitigation;
            this.AssignADUserName = assignADUserName;
            this.AssignWorkflowRoleTypeKey = assignWorkflowRoleTypeKey;
            this.PreviousState = previousState;
            this.CourtCase = courtCase;
            this.LatestReasonDefinitionKey = latestReasonDefinitionKey;
            this.ProductKey = productKey;
            this.GenericKey = genericKey;
		
        }		

        public long InstanceID { get; set; }

        public int? DebtCounsellingKey { get; set; }

        public int? AccountKey { get; set; }

        public bool? SentToLitigation { get; set; }

        public string AssignADUserName { get; set; }

        public int? AssignWorkflowRoleTypeKey { get; set; }

        public string PreviousState { get; set; }

        public bool? CourtCase { get; set; }

        public int? LatestReasonDefinitionKey { get; set; }

        public int ProductKey { get; set; }

        public int GenericKey { get; set; }
    }
}