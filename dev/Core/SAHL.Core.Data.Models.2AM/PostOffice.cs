using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class PostOfficeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public PostOfficeDataModel(string description, string postalCode, int? cityKey)
        {
            this.Description = description;
            this.PostalCode = postalCode;
            this.CityKey = cityKey;
		
        }
		[JsonConstructor]
        public PostOfficeDataModel(int postOfficeKey, string description, string postalCode, int? cityKey)
        {
            this.PostOfficeKey = postOfficeKey;
            this.Description = description;
            this.PostalCode = postalCode;
            this.CityKey = cityKey;
		
        }		

        public int PostOfficeKey { get; set; }

        public string Description { get; set; }

        public string PostalCode { get; set; }

        public int? CityKey { get; set; }

        public void SetKey(int key)
        {
            this.PostOfficeKey =  key;
        }
    }
}