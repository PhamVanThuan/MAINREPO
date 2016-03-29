using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RateConfigurationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public RateConfigurationDataModel(int marketRateKey, int marginKey)
        {
            this.MarketRateKey = marketRateKey;
            this.MarginKey = marginKey;
		
        }
		[JsonConstructor]
        public RateConfigurationDataModel(int rateConfigurationKey, int marketRateKey, int marginKey)
        {
            this.RateConfigurationKey = rateConfigurationKey;
            this.MarketRateKey = marketRateKey;
            this.MarginKey = marginKey;
		
        }		

        public int RateConfigurationKey { get; set; }

        public int MarketRateKey { get; set; }

        public int MarginKey { get; set; }

        public void SetKey(int key)
        {
            this.RateConfigurationKey =  key;
        }
    }
}