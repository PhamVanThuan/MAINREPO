using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OriginationSourceProductDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OriginationSourceProductDataModel(int originationSourceKey, int productKey)
        {
            this.OriginationSourceKey = originationSourceKey;
            this.ProductKey = productKey;
		
        }
		[JsonConstructor]
        public OriginationSourceProductDataModel(int originationSourceProductKey, int originationSourceKey, int productKey)
        {
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.OriginationSourceKey = originationSourceKey;
            this.ProductKey = productKey;
		
        }		

        public int OriginationSourceProductKey { get; set; }

        public int OriginationSourceKey { get; set; }

        public int ProductKey { get; set; }

        public void SetKey(int key)
        {
            this.OriginationSourceProductKey =  key;
        }
    }
}