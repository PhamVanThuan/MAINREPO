using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DeedsOfficeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DeedsOfficeDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public DeedsOfficeDataModel(int deedsOfficeKey, string description)
        {
            this.DeedsOfficeKey = deedsOfficeKey;
            this.Description = description;
		
        }		

        public int DeedsOfficeKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.DeedsOfficeKey =  key;
        }
    }
}