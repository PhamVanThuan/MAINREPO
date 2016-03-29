using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class BusinessRuleAreaDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public BusinessRuleAreaDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public BusinessRuleAreaDataModel(int businessRuleAreaKey, string description)
        {
            this.BusinessRuleAreaKey = businessRuleAreaKey;
            this.Description = description;
		
        }		

        public int BusinessRuleAreaKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.BusinessRuleAreaKey =  key;
        }
    }
}