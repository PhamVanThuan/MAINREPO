using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditOriginationSourceProductConfigurationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditOriginationSourceProductConfigurationDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int? oSPConfigurationKey, int? originationSourceProductKey, int? financialServiceTypeKey, int? marketRateKey, int? resetConfigurationKey, decimal? discountPercentage)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.OSPConfigurationKey = oSPConfigurationKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.FinancialServiceTypeKey = financialServiceTypeKey;
            this.MarketRateKey = marketRateKey;
            this.ResetConfigurationKey = resetConfigurationKey;
            this.DiscountPercentage = discountPercentage;
		
        }
		[JsonConstructor]
        public AuditOriginationSourceProductConfigurationDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int? oSPConfigurationKey, int? originationSourceProductKey, int? financialServiceTypeKey, int? marketRateKey, int? resetConfigurationKey, decimal? discountPercentage)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.OSPConfigurationKey = oSPConfigurationKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.FinancialServiceTypeKey = financialServiceTypeKey;
            this.MarketRateKey = marketRateKey;
            this.ResetConfigurationKey = resetConfigurationKey;
            this.DiscountPercentage = discountPercentage;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int? OSPConfigurationKey { get; set; }

        public int? OriginationSourceProductKey { get; set; }

        public int? FinancialServiceTypeKey { get; set; }

        public int? MarketRateKey { get; set; }

        public int? ResetConfigurationKey { get; set; }

        public decimal? DiscountPercentage { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}