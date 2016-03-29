using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DataProviderDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DataProviderDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public DataProviderDataModel(int dataProviderKey, string description)
        {
            this.DataProviderKey = dataProviderKey;
            this.Description = description;
		
        }		

        public int DataProviderKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.DataProviderKey =  key;
        }
    }
}