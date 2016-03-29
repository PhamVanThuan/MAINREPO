using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CreditCriteriaAttributeTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CreditCriteriaAttributeTypeDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public CreditCriteriaAttributeTypeDataModel(int creditCriteriaAttributeTypeKey, string description)
        {
            this.CreditCriteriaAttributeTypeKey = creditCriteriaAttributeTypeKey;
            this.Description = description;
		
        }		

        public int CreditCriteriaAttributeTypeKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.CreditCriteriaAttributeTypeKey =  key;
        }
    }
}