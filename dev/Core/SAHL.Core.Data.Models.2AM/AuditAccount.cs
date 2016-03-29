using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditAccountDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditAccountDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int accountKey, double fixedPayment, int accountStatusKey, DateTime? insertedDate, int? originationSourceProductKey, DateTime? openDate, DateTime? closeDate, int? rRR_ProductKey, int? rRR_OriginationSourceKey, string userID, DateTime? changeDate, int? sPVKey, int? parentAccountKey)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.AccountKey = accountKey;
            this.FixedPayment = fixedPayment;
            this.AccountStatusKey = accountStatusKey;
            this.InsertedDate = insertedDate;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.OpenDate = openDate;
            this.CloseDate = closeDate;
            this.RRR_ProductKey = rRR_ProductKey;
            this.RRR_OriginationSourceKey = rRR_OriginationSourceKey;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.SPVKey = sPVKey;
            this.ParentAccountKey = parentAccountKey;
		
        }
		[JsonConstructor]
        public AuditAccountDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int accountKey, double fixedPayment, int accountStatusKey, DateTime? insertedDate, int? originationSourceProductKey, DateTime? openDate, DateTime? closeDate, int? rRR_ProductKey, int? rRR_OriginationSourceKey, string userID, DateTime? changeDate, int? sPVKey, int? parentAccountKey)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.AccountKey = accountKey;
            this.FixedPayment = fixedPayment;
            this.AccountStatusKey = accountStatusKey;
            this.InsertedDate = insertedDate;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.OpenDate = openDate;
            this.CloseDate = closeDate;
            this.RRR_ProductKey = rRR_ProductKey;
            this.RRR_OriginationSourceKey = rRR_OriginationSourceKey;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.SPVKey = sPVKey;
            this.ParentAccountKey = parentAccountKey;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int AccountKey { get; set; }

        public double FixedPayment { get; set; }

        public int AccountStatusKey { get; set; }

        public DateTime? InsertedDate { get; set; }

        public int? OriginationSourceProductKey { get; set; }

        public DateTime? OpenDate { get; set; }

        public DateTime? CloseDate { get; set; }

        public int? RRR_ProductKey { get; set; }

        public int? RRR_OriginationSourceKey { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public int? SPVKey { get; set; }

        public int? ParentAccountKey { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}