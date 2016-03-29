using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditBankAccountDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditBankAccountDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int bankAccountKey, string aCBBranchCode, string accountNumber, int aCBTypeNumber, string accountName, string userID, DateTime? changeDate)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.BankAccountKey = bankAccountKey;
            this.ACBBranchCode = aCBBranchCode;
            this.AccountNumber = accountNumber;
            this.ACBTypeNumber = aCBTypeNumber;
            this.AccountName = accountName;
            this.UserID = userID;
            this.ChangeDate = changeDate;
		
        }
		[JsonConstructor]
        public AuditBankAccountDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int bankAccountKey, string aCBBranchCode, string accountNumber, int aCBTypeNumber, string accountName, string userID, DateTime? changeDate)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.BankAccountKey = bankAccountKey;
            this.ACBBranchCode = aCBBranchCode;
            this.AccountNumber = accountNumber;
            this.ACBTypeNumber = aCBTypeNumber;
            this.AccountName = accountName;
            this.UserID = userID;
            this.ChangeDate = changeDate;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int BankAccountKey { get; set; }

        public string ACBBranchCode { get; set; }

        public string AccountNumber { get; set; }

        public int ACBTypeNumber { get; set; }

        public string AccountName { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}