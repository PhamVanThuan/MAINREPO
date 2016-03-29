using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class FunctionalGroupDefinitionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public FunctionalGroupDefinitionDataModel(string functionalGroupName, int genericKeyTypeKey, bool allowMany)
        {
            this.FunctionalGroupName = functionalGroupName;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.AllowMany = allowMany;
		
        }
		[JsonConstructor]
        public FunctionalGroupDefinitionDataModel(int functionalGroupDefinitionKey, string functionalGroupName, int genericKeyTypeKey, bool allowMany)
        {
            this.FunctionalGroupDefinitionKey = functionalGroupDefinitionKey;
            this.FunctionalGroupName = functionalGroupName;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.AllowMany = allowMany;
		
        }		

        public int FunctionalGroupDefinitionKey { get; set; }

        public string FunctionalGroupName { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public bool AllowMany { get; set; }

        public void SetKey(int key)
        {
            this.FunctionalGroupDefinitionKey =  key;
        }
    }
}