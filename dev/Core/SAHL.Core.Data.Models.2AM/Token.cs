using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class TokenDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public TokenDataModel(string description, int tokenTypeKey, int? statementDefinitionKey, bool mustTranslate, int parameterTypeKey)
        {
            this.Description = description;
            this.TokenTypeKey = tokenTypeKey;
            this.StatementDefinitionKey = statementDefinitionKey;
            this.MustTranslate = mustTranslate;
            this.ParameterTypeKey = parameterTypeKey;
		
        }
		[JsonConstructor]
        public TokenDataModel(int tokenKey, string description, int tokenTypeKey, int? statementDefinitionKey, bool mustTranslate, int parameterTypeKey)
        {
            this.TokenKey = tokenKey;
            this.Description = description;
            this.TokenTypeKey = tokenTypeKey;
            this.StatementDefinitionKey = statementDefinitionKey;
            this.MustTranslate = mustTranslate;
            this.ParameterTypeKey = parameterTypeKey;
		
        }		

        public int TokenKey { get; set; }

        public string Description { get; set; }

        public int TokenTypeKey { get; set; }

        public int? StatementDefinitionKey { get; set; }

        public bool MustTranslate { get; set; }

        public int ParameterTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.TokenKey =  key;
        }
    }
}