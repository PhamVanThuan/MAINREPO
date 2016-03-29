using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditAccountPropertyDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditAccountPropertyDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int? accountPropertyKey, int? accountKey, int? propertyKey)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.AccountPropertyKey = accountPropertyKey;
            this.AccountKey = accountKey;
            this.PropertyKey = propertyKey;
		
        }
		[JsonConstructor]
        public AuditAccountPropertyDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int? accountPropertyKey, int? accountKey, int? propertyKey)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.AccountPropertyKey = accountPropertyKey;
            this.AccountKey = accountKey;
            this.PropertyKey = propertyKey;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int? AccountPropertyKey { get; set; }

        public int? AccountKey { get; set; }

        public int? PropertyKey { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}