using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class MarginProductDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public MarginProductDataModel(int? marginKey, int? originationSourceProductKey, decimal? discount)
        {
            this.MarginKey = marginKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.Discount = discount;
		
        }
		[JsonConstructor]
        public MarginProductDataModel(int marginProductKey, int? marginKey, int? originationSourceProductKey, decimal? discount)
        {
            this.MarginProductKey = marginProductKey;
            this.MarginKey = marginKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.Discount = discount;
		
        }		

        public int MarginProductKey { get; set; }

        public int? MarginKey { get; set; }

        public int? OriginationSourceProductKey { get; set; }

        public decimal? Discount { get; set; }

        public void SetKey(int key)
        {
            this.MarginProductKey =  key;
        }
    }
}