using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LifeCommissionRatesDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LifeCommissionRatesDataModel(string entity, double percentage, DateTime effectiveDate)
        {
            this.Entity = entity;
            this.Percentage = percentage;
            this.EffectiveDate = effectiveDate;
		
        }
		[JsonConstructor]
        public LifeCommissionRatesDataModel(int ratesKey, string entity, double percentage, DateTime effectiveDate)
        {
            this.RatesKey = ratesKey;
            this.Entity = entity;
            this.Percentage = percentage;
            this.EffectiveDate = effectiveDate;
		
        }		

        public int RatesKey { get; set; }

        public string Entity { get; set; }

        public double Percentage { get; set; }

        public DateTime EffectiveDate { get; set; }

        public void SetKey(int key)
        {
            this.RatesKey =  key;
        }
    }
}