using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ProductCategoryDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ProductCategoryDataModel(int? originationSourceProductKey, int? marginKey, int? categoryKey)
        {
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.MarginKey = marginKey;
            this.CategoryKey = categoryKey;
		
        }
		[JsonConstructor]
        public ProductCategoryDataModel(int productCategoryKey, int? originationSourceProductKey, int? marginKey, int? categoryKey)
        {
            this.ProductCategoryKey = productCategoryKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.MarginKey = marginKey;
            this.CategoryKey = categoryKey;
		
        }		

        public int ProductCategoryKey { get; set; }

        public int? OriginationSourceProductKey { get; set; }

        public int? MarginKey { get; set; }

        public int? CategoryKey { get; set; }

        public void SetKey(int key)
        {
            this.ProductCategoryKey =  key;
        }
    }
}