using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class GenericSetTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public GenericSetTypeDataModel(string description, int genericKeyTypeKey)
        {
            this.Description = description;
            this.GenericKeyTypeKey = genericKeyTypeKey;
		
        }
		[JsonConstructor]
        public GenericSetTypeDataModel(int genericSetTypeKey, string description, int genericKeyTypeKey)
        {
            this.GenericSetTypeKey = genericSetTypeKey;
            this.Description = description;
            this.GenericKeyTypeKey = genericKeyTypeKey;
		
        }		

        public int GenericSetTypeKey { get; set; }

        public string Description { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.GenericSetTypeKey =  key;
        }
    }
}