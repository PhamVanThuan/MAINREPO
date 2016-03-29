using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class GenericSetDefinitionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public GenericSetDefinitionDataModel(string description, int genericSetTypeKey, string explanation)
        {
            this.Description = description;
            this.GenericSetTypeKey = genericSetTypeKey;
            this.Explanation = explanation;
		
        }
		[JsonConstructor]
        public GenericSetDefinitionDataModel(int genericSetDefinitionKey, string description, int genericSetTypeKey, string explanation)
        {
            this.GenericSetDefinitionKey = genericSetDefinitionKey;
            this.Description = description;
            this.GenericSetTypeKey = genericSetTypeKey;
            this.Explanation = explanation;
		
        }		

        public int GenericSetDefinitionKey { get; set; }

        public string Description { get; set; }

        public int GenericSetTypeKey { get; set; }

        public string Explanation { get; set; }

        public void SetKey(int key)
        {
            this.GenericSetDefinitionKey =  key;
        }
    }
}