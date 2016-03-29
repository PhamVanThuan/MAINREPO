using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class StatementDefinitionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public StatementDefinitionDataModel(string description, string applicationName, string statementName)
        {
            this.Description = description;
            this.ApplicationName = applicationName;
            this.StatementName = statementName;
		
        }
		[JsonConstructor]
        public StatementDefinitionDataModel(int statementDefinitionKey, string description, string applicationName, string statementName)
        {
            this.StatementDefinitionKey = statementDefinitionKey;
            this.Description = description;
            this.ApplicationName = applicationName;
            this.StatementName = statementName;
		
        }		

        public int StatementDefinitionKey { get; set; }

        public string Description { get; set; }

        public string ApplicationName { get; set; }

        public string StatementName { get; set; }

        public void SetKey(int key)
        {
            this.StatementDefinitionKey =  key;
        }
    }
}