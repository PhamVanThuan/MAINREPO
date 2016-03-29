using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RoundRobinPointerDefinitionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public RoundRobinPointerDefinitionDataModel(int roundRobinPointerKey, int genericKeyTypeKey, int genericKey, string applicationName, string statementName, int generalStatusKey)
        {
            this.RoundRobinPointerKey = roundRobinPointerKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericKey = genericKey;
            this.ApplicationName = applicationName;
            this.StatementName = statementName;
            this.GeneralStatusKey = generalStatusKey;
		
        }
		[JsonConstructor]
        public RoundRobinPointerDefinitionDataModel(int roundRobinPointerDefinitionKey, int roundRobinPointerKey, int genericKeyTypeKey, int genericKey, string applicationName, string statementName, int generalStatusKey)
        {
            this.RoundRobinPointerDefinitionKey = roundRobinPointerDefinitionKey;
            this.RoundRobinPointerKey = roundRobinPointerKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericKey = genericKey;
            this.ApplicationName = applicationName;
            this.StatementName = statementName;
            this.GeneralStatusKey = generalStatusKey;
		
        }		

        public int RoundRobinPointerDefinitionKey { get; set; }

        public int RoundRobinPointerKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public int GenericKey { get; set; }

        public string ApplicationName { get; set; }

        public string StatementName { get; set; }

        public int GeneralStatusKey { get; set; }

        public void SetKey(int key)
        {
            this.RoundRobinPointerDefinitionKey =  key;
        }
    }
}