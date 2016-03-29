using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditLegalEntityBankAccountDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditLegalEntityBankAccountDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int legalEntityKey, int bankAccountKey, int legalEntityBankAccountKey, int generalStatusKey, string userID, DateTime? changeDate)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.LegalEntityKey = legalEntityKey;
            this.BankAccountKey = bankAccountKey;
            this.LegalEntityBankAccountKey = legalEntityBankAccountKey;
            this.GeneralStatusKey = generalStatusKey;
            this.UserID = userID;
            this.ChangeDate = changeDate;
		
        }
		[JsonConstructor]
        public AuditLegalEntityBankAccountDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int legalEntityKey, int bankAccountKey, int legalEntityBankAccountKey, int generalStatusKey, string userID, DateTime? changeDate)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.LegalEntityKey = legalEntityKey;
            this.BankAccountKey = bankAccountKey;
            this.LegalEntityBankAccountKey = legalEntityBankAccountKey;
            this.GeneralStatusKey = generalStatusKey;
            this.UserID = userID;
            this.ChangeDate = changeDate;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int LegalEntityKey { get; set; }

        public int BankAccountKey { get; set; }

        public int LegalEntityBankAccountKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}