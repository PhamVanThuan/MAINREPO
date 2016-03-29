using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CreditCriteriaAttributeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CreditCriteriaAttributeDataModel(int creditCriteriaKey, int creditCriteriaAttributeTypeKey)
        {
            this.CreditCriteriaKey = creditCriteriaKey;
            this.CreditCriteriaAttributeTypeKey = creditCriteriaAttributeTypeKey;
		
        }
		[JsonConstructor]
        public CreditCriteriaAttributeDataModel(int creditCriteriaAttributeKey, int creditCriteriaKey, int creditCriteriaAttributeTypeKey)
        {
            this.CreditCriteriaAttributeKey = creditCriteriaAttributeKey;
            this.CreditCriteriaKey = creditCriteriaKey;
            this.CreditCriteriaAttributeTypeKey = creditCriteriaAttributeTypeKey;
		
        }		

        public int CreditCriteriaAttributeKey { get; set; }

        public int CreditCriteriaKey { get; set; }

        public int CreditCriteriaAttributeTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.CreditCriteriaAttributeKey =  key;
        }
    }
}