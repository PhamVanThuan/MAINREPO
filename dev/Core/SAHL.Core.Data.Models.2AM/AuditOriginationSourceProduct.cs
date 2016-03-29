using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditOriginationSourceProductDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditOriginationSourceProductDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int? originationSourceProductKey, int originationSourceKey, int productKey)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.OriginationSourceKey = originationSourceKey;
            this.ProductKey = productKey;
		
        }
		[JsonConstructor]
        public AuditOriginationSourceProductDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int? originationSourceProductKey, int originationSourceKey, int productKey)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.OriginationSourceKey = originationSourceKey;
            this.ProductKey = productKey;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int? OriginationSourceProductKey { get; set; }

        public int OriginationSourceKey { get; set; }

        public int ProductKey { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}