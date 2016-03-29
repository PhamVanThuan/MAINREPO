using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CityDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CityDataModel(string description, int provinceKey)
        {
            this.Description = description;
            this.ProvinceKey = provinceKey;
		
        }
		[JsonConstructor]
        public CityDataModel(int cityKey, string description, int provinceKey)
        {
            this.CityKey = cityKey;
            this.Description = description;
            this.ProvinceKey = provinceKey;
		
        }		

        public int CityKey { get; set; }

        public string Description { get; set; }

        public int ProvinceKey { get; set; }

        public void SetKey(int key)
        {
            this.CityKey =  key;
        }
    }
}