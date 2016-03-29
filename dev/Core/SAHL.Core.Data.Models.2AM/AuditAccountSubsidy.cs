using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditAccountSubsidyDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditAccountSubsidyDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int accountSubsidyKey, int accountKey, int subsidyKey)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.AccountSubsidyKey = accountSubsidyKey;
            this.AccountKey = accountKey;
            this.SubsidyKey = subsidyKey;
		
        }
		[JsonConstructor]
        public AuditAccountSubsidyDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int accountSubsidyKey, int accountKey, int subsidyKey)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.AccountSubsidyKey = accountSubsidyKey;
            this.AccountKey = accountKey;
            this.SubsidyKey = subsidyKey;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int AccountSubsidyKey { get; set; }

        public int AccountKey { get; set; }

        public int SubsidyKey { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}