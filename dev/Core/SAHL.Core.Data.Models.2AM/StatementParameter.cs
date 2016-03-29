using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class StatementParameterDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public StatementParameterDataModel(string parameterName, int parameterTypeKey, int statementDefinitionKey, int? parameterLength, string displayName, bool required, int? populationStatementDefinitionKey)
        {
            this.ParameterName = parameterName;
            this.ParameterTypeKey = parameterTypeKey;
            this.StatementDefinitionKey = statementDefinitionKey;
            this.ParameterLength = parameterLength;
            this.DisplayName = displayName;
            this.Required = required;
            this.PopulationStatementDefinitionKey = populationStatementDefinitionKey;
		
        }
		[JsonConstructor]
        public StatementParameterDataModel(int statementParameterKey, string parameterName, int parameterTypeKey, int statementDefinitionKey, int? parameterLength, string displayName, bool required, int? populationStatementDefinitionKey)
        {
            this.StatementParameterKey = statementParameterKey;
            this.ParameterName = parameterName;
            this.ParameterTypeKey = parameterTypeKey;
            this.StatementDefinitionKey = statementDefinitionKey;
            this.ParameterLength = parameterLength;
            this.DisplayName = displayName;
            this.Required = required;
            this.PopulationStatementDefinitionKey = populationStatementDefinitionKey;
		
        }		

        public int StatementParameterKey { get; set; }

        public string ParameterName { get; set; }

        public int ParameterTypeKey { get; set; }

        public int StatementDefinitionKey { get; set; }

        public int? ParameterLength { get; set; }

        public string DisplayName { get; set; }

        public bool Required { get; set; }

        public int? PopulationStatementDefinitionKey { get; set; }

        public void SetKey(int key)
        {
            this.StatementParameterKey =  key;
        }
    }
}