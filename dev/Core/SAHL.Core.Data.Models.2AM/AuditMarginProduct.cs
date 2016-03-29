using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditMarginProductDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditMarginProductDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int? marginProductKey, int? marginKey, int? originationSourceProductKey, decimal? discount)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.MarginProductKey = marginProductKey;
            this.MarginKey = marginKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.Discount = discount;
		
        }
		[JsonConstructor]
        public AuditMarginProductDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int? marginProductKey, int? marginKey, int? originationSourceProductKey, decimal? discount)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.MarginProductKey = marginProductKey;
            this.MarginKey = marginKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.Discount = discount;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int? MarginProductKey { get; set; }

        public int? MarginKey { get; set; }

        public int? OriginationSourceProductKey { get; set; }

        public decimal? Discount { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}