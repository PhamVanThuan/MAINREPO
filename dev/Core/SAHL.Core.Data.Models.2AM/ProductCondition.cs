using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ProductConditionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ProductConditionDataModel(int? productConditionTypeKey, int? conditionKey, int? originationSourceProductKey, int? financialServiceTypeKey, int? purposeKey, string applicationName)
        {
            this.ProductConditionTypeKey = productConditionTypeKey;
            this.ConditionKey = conditionKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.FinancialServiceTypeKey = financialServiceTypeKey;
            this.PurposeKey = purposeKey;
            this.ApplicationName = applicationName;
		
        }
		[JsonConstructor]
        public ProductConditionDataModel(int productConditionKey, int? productConditionTypeKey, int? conditionKey, int? originationSourceProductKey, int? financialServiceTypeKey, int? purposeKey, string applicationName)
        {
            this.ProductConditionKey = productConditionKey;
            this.ProductConditionTypeKey = productConditionTypeKey;
            this.ConditionKey = conditionKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.FinancialServiceTypeKey = financialServiceTypeKey;
            this.PurposeKey = purposeKey;
            this.ApplicationName = applicationName;
		
        }		

        public int ProductConditionKey { get; set; }

        public int? ProductConditionTypeKey { get; set; }

        public int? ConditionKey { get; set; }

        public int? OriginationSourceProductKey { get; set; }

        public int? FinancialServiceTypeKey { get; set; }

        public int? PurposeKey { get; set; }

        public string ApplicationName { get; set; }

        public void SetKey(int key)
        {
            this.ProductConditionKey =  key;
        }
    }
}