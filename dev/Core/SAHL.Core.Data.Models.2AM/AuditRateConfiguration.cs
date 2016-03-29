using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditRateConfigurationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditRateConfigurationDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int? rateConfigurationKey, int marketRateKey, int marginKey)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.RateConfigurationKey = rateConfigurationKey;
            this.MarketRateKey = marketRateKey;
            this.MarginKey = marginKey;
		
        }
		[JsonConstructor]
        public AuditRateConfigurationDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int? rateConfigurationKey, int marketRateKey, int marginKey)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.RateConfigurationKey = rateConfigurationKey;
            this.MarketRateKey = marketRateKey;
            this.MarginKey = marginKey;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int? RateConfigurationKey { get; set; }

        public int MarketRateKey { get; set; }

        public int MarginKey { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}