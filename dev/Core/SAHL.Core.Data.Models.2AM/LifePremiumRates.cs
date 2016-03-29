using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LifePremiumRatesDataModel :  IDataModel
    {
        public LifePremiumRatesDataModel(int nextAge, decimal? death, int lifePolicyTypeKey)
        {
            this.NextAge = nextAge;
            this.Death = death;
            this.LifePolicyTypeKey = lifePolicyTypeKey;
		
        }		

        public int NextAge { get; set; }

        public decimal? Death { get; set; }

        public int LifePolicyTypeKey { get; set; }
    }
}