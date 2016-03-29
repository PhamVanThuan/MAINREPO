using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditRoleDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditRoleDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int legalEntityKey, int accountKey, int roleTypeKey, int accountRoleKey, int generalStatusKey, DateTime statusChangeDate)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.LegalEntityKey = legalEntityKey;
            this.AccountKey = accountKey;
            this.RoleTypeKey = roleTypeKey;
            this.AccountRoleKey = accountRoleKey;
            this.GeneralStatusKey = generalStatusKey;
            this.StatusChangeDate = statusChangeDate;
		
        }
		[JsonConstructor]
        public AuditRoleDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int legalEntityKey, int accountKey, int roleTypeKey, int accountRoleKey, int generalStatusKey, DateTime statusChangeDate)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.LegalEntityKey = legalEntityKey;
            this.AccountKey = accountKey;
            this.RoleTypeKey = roleTypeKey;
            this.AccountRoleKey = accountRoleKey;
            this.GeneralStatusKey = generalStatusKey;
            this.StatusChangeDate = statusChangeDate;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int LegalEntityKey { get; set; }

        public int AccountKey { get; set; }

        public int RoleTypeKey { get; set; }

        public int AccountRoleKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public DateTime StatusChangeDate { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}