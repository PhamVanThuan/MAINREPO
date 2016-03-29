using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ConditionTokenDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ConditionTokenDataModel(int conditionKey, int tokenKey)
        {
            this.ConditionKey = conditionKey;
            this.TokenKey = tokenKey;
		
        }
		[JsonConstructor]
        public ConditionTokenDataModel(int conditionTokenKey, int conditionKey, int tokenKey)
        {
            this.ConditionTokenKey = conditionTokenKey;
            this.ConditionKey = conditionKey;
            this.TokenKey = tokenKey;
		
        }		

        public int ConditionTokenKey { get; set; }

        public int ConditionKey { get; set; }

        public int TokenKey { get; set; }

        public void SetKey(int key)
        {
            this.ConditionTokenKey =  key;
        }
    }
}