using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class SuburbDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public SuburbDataModel(string description, int cityKey, string postalCode)
        {
            this.Description = description;
            this.CityKey = cityKey;
            this.PostalCode = postalCode;
		
        }
		[JsonConstructor]
        public SuburbDataModel(int suburbKey, string description, int cityKey, string postalCode)
        {
            this.SuburbKey = suburbKey;
            this.Description = description;
            this.CityKey = cityKey;
            this.PostalCode = postalCode;
		
        }		

        public int SuburbKey { get; set; }

        public string Description { get; set; }

        public int CityKey { get; set; }

        public string PostalCode { get; set; }

        public void SetKey(int key)
        {
            this.SuburbKey =  key;
        }
    }
}