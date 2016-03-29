using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DataTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DataTypeDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public DataTypeDataModel(int dataTypeKey, string description)
        {
            this.DataTypeKey = dataTypeKey;
            this.Description = description;
		
        }		

        public int DataTypeKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.DataTypeKey =  key;
        }
    }
}