using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class FinancialServiceConditionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public FinancialServiceConditionDataModel(int? financialServiceKey, string userDefinedConditionText, int? conditionTypeKey, int? conditionKey)
        {
            this.FinancialServiceKey = financialServiceKey;
            this.UserDefinedConditionText = userDefinedConditionText;
            this.ConditionTypeKey = conditionTypeKey;
            this.ConditionKey = conditionKey;
		
        }
		[JsonConstructor]
        public FinancialServiceConditionDataModel(int financialServiceConditionKey, int? financialServiceKey, string userDefinedConditionText, int? conditionTypeKey, int? conditionKey)
        {
            this.FinancialServiceConditionKey = financialServiceConditionKey;
            this.FinancialServiceKey = financialServiceKey;
            this.UserDefinedConditionText = userDefinedConditionText;
            this.ConditionTypeKey = conditionTypeKey;
            this.ConditionKey = conditionKey;
		
        }		

        public int FinancialServiceConditionKey { get; set; }

        public int? FinancialServiceKey { get; set; }

        public string UserDefinedConditionText { get; set; }

        public int? ConditionTypeKey { get; set; }

        public int? ConditionKey { get; set; }

        public void SetKey(int key)
        {
            this.FinancialServiceConditionKey =  key;
        }
    }
}