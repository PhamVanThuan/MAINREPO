using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditPropertyTitleDeedDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditPropertyTitleDeedDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int propertyTitleDeedKey, int propertyKey, string titleDeedNumber, int? deedsOfficeKey)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.PropertyTitleDeedKey = propertyTitleDeedKey;
            this.PropertyKey = propertyKey;
            this.TitleDeedNumber = titleDeedNumber;
            this.DeedsOfficeKey = deedsOfficeKey;
		
        }
		[JsonConstructor]
        public AuditPropertyTitleDeedDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int propertyTitleDeedKey, int propertyKey, string titleDeedNumber, int? deedsOfficeKey)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.PropertyTitleDeedKey = propertyTitleDeedKey;
            this.PropertyKey = propertyKey;
            this.TitleDeedNumber = titleDeedNumber;
            this.DeedsOfficeKey = deedsOfficeKey;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int PropertyTitleDeedKey { get; set; }

        public int PropertyKey { get; set; }

        public string TitleDeedNumber { get; set; }

        public int? DeedsOfficeKey { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}