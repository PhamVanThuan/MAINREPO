using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class Loan_AdjustmentsDataModel :  IDataModel
    {
        public Loan_AdjustmentsDataModel(long instanceID, int? accountKey, int? sPVKey, string requestUser, string processUser, bool? requestApproved, int? loanAdjustmentType, int? newTerm, int? oldTerm, int genericKey)
        {
            this.InstanceID = instanceID;
            this.AccountKey = accountKey;
            this.SPVKey = sPVKey;
            this.RequestUser = requestUser;
            this.ProcessUser = processUser;
            this.RequestApproved = requestApproved;
            this.LoanAdjustmentType = loanAdjustmentType;
            this.NewTerm = newTerm;
            this.OldTerm = oldTerm;
            this.GenericKey = genericKey;
		
        }		

        public long InstanceID { get; set; }

        public int? AccountKey { get; set; }

        public int? SPVKey { get; set; }

        public string RequestUser { get; set; }

        public string ProcessUser { get; set; }

        public bool? RequestApproved { get; set; }

        public int? LoanAdjustmentType { get; set; }

        public int? NewTerm { get; set; }

        public int? OldTerm { get; set; }

        public int GenericKey { get; set; }
    }
}