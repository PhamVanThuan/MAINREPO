using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OriginationSourceProductConfigurationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OriginationSourceProductConfigurationDataModel(int? originationSourceProductKey, int? financialServiceTypeKey, int? marketRateKey, int? resetConfigurationKey)
        {
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.FinancialServiceTypeKey = financialServiceTypeKey;
            this.MarketRateKey = marketRateKey;
            this.ResetConfigurationKey = resetConfigurationKey;
		
        }
		[JsonConstructor]
        public OriginationSourceProductConfigurationDataModel(int oSPConfigurationKey, int? originationSourceProductKey, int? financialServiceTypeKey, int? marketRateKey, int? resetConfigurationKey)
        {
            this.OSPConfigurationKey = oSPConfigurationKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.FinancialServiceTypeKey = financialServiceTypeKey;
            this.MarketRateKey = marketRateKey;
            this.ResetConfigurationKey = resetConfigurationKey;
		
        }		

        public int OSPConfigurationKey { get; set; }

        public int? OriginationSourceProductKey { get; set; }

        public int? FinancialServiceTypeKey { get; set; }

        public int? MarketRateKey { get; set; }

        public int? ResetConfigurationKey { get; set; }

        public void SetKey(int key)
        {
            this.OSPConfigurationKey =  key;
        }
    }
}