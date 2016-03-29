using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class GenericSetDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public GenericSetDataModel(int genericSetDefinitionKey, int genericKey)
        {
            this.GenericSetDefinitionKey = genericSetDefinitionKey;
            this.GenericKey = genericKey;
		
        }
		[JsonConstructor]
        public GenericSetDataModel(int genericSetKey, int genericSetDefinitionKey, int genericKey)
        {
            this.GenericSetKey = genericSetKey;
            this.GenericSetDefinitionKey = genericSetDefinitionKey;
            this.GenericKey = genericKey;
		
        }		

        public int GenericSetKey { get; set; }

        public int GenericSetDefinitionKey { get; set; }

        public int GenericKey { get; set; }

        public void SetKey(int key)
        {
            this.GenericSetKey =  key;
        }
    }
}