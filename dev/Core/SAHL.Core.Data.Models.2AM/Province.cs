using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ProvinceDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ProvinceDataModel(string description, int countryKey)
        {
            this.Description = description;
            this.CountryKey = countryKey;
		
        }
		[JsonConstructor]
        public ProvinceDataModel(int provinceKey, string description, int countryKey)
        {
            this.ProvinceKey = provinceKey;
            this.Description = description;
            this.CountryKey = countryKey;
		
        }		

        public int ProvinceKey { get; set; }

        public string Description { get; set; }

        public int CountryKey { get; set; }

        public void SetKey(int key)
        {
            this.ProvinceKey =  key;
        }
    }
}