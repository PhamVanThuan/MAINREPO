using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class MarketRateDataModel :  IDataModel
    {
        public MarketRateDataModel(int marketRateKey, double value, string description)
        {
            this.MarketRateKey = marketRateKey;
            this.Value = value;
            this.Description = description;
		
        }		

        public int MarketRateKey { get; set; }

        public double Value { get; set; }

        public string Description { get; set; }
    }
}