using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class TokenConditionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public TokenConditionDataModel(string token, string statementName, string applicationName)
        {
            this.Token = token;
            this.StatementName = statementName;
            this.ApplicationName = applicationName;
		
        }
		[JsonConstructor]
        public TokenConditionDataModel(int tokenKey, string token, string statementName, string applicationName)
        {
            this.TokenKey = tokenKey;
            this.Token = token;
            this.StatementName = statementName;
            this.ApplicationName = applicationName;
		
        }		

        public int TokenKey { get; set; }

        public string Token { get; set; }

        public string StatementName { get; set; }

        public string ApplicationName { get; set; }

        public void SetKey(int key)
        {
            this.TokenKey =  key;
        }
    }
}