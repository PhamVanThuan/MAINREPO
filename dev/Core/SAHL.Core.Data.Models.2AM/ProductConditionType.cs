using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ProductConditionTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ProductConditionTypeDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public ProductConditionTypeDataModel(int productConditionTypeKey, string description)
        {
            this.ProductConditionTypeKey = productConditionTypeKey;
            this.Description = description;
		
        }		

        public int ProductConditionTypeKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.ProductConditionTypeKey =  key;
        }
    }
}