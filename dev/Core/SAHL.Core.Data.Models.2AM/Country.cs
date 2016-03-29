using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CountryDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CountryDataModel(string description, bool allowFreeTextFormat)
        {
            this.Description = description;
            this.AllowFreeTextFormat = allowFreeTextFormat;
		
        }
		[JsonConstructor]
        public CountryDataModel(int countryKey, string description, bool allowFreeTextFormat)
        {
            this.CountryKey = countryKey;
            this.Description = description;
            this.AllowFreeTextFormat = allowFreeTextFormat;
		
        }		

        public int CountryKey { get; set; }

        public string Description { get; set; }

        public bool AllowFreeTextFormat { get; set; }

        public void SetKey(int key)
        {
            this.CountryKey =  key;
        }
    }
}