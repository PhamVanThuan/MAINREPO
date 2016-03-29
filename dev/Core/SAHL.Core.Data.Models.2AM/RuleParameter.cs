using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RuleParameterDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public RuleParameterDataModel(int ruleItemKey, string name, int parameterTypeKey, string value)
        {
            this.RuleItemKey = ruleItemKey;
            this.Name = name;
            this.ParameterTypeKey = parameterTypeKey;
            this.Value = value;
		
        }
		[JsonConstructor]
        public RuleParameterDataModel(int ruleParameterKey, int ruleItemKey, string name, int parameterTypeKey, string value)
        {
            this.RuleParameterKey = ruleParameterKey;
            this.RuleItemKey = ruleItemKey;
            this.Name = name;
            this.ParameterTypeKey = parameterTypeKey;
            this.Value = value;
		
        }		

        public int RuleParameterKey { get; set; }

        public int RuleItemKey { get; set; }

        public string Name { get; set; }

        public int ParameterTypeKey { get; set; }

        public string Value { get; set; }

        public void SetKey(int key)
        {
            this.RuleParameterKey =  key;
        }
    }
}