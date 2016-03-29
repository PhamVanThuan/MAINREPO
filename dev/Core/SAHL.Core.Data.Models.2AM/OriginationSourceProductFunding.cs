using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OriginationSourceProductFundingDataModel :  IDataModel
    {
        public OriginationSourceProductFundingDataModel(int originationSourceKey, int productKey, int originationSourceProductKey, double lTV, double linkRate, int fundingWarehouse)
        {
            this.OriginationSourceKey = originationSourceKey;
            this.ProductKey = productKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.LTV = lTV;
            this.LinkRate = linkRate;
            this.FundingWarehouse = fundingWarehouse;
		
        }		

        public int OriginationSourceKey { get; set; }

        public int ProductKey { get; set; }

        public int OriginationSourceProductKey { get; set; }

        public double LTV { get; set; }

        public double LinkRate { get; set; }

        public int FundingWarehouse { get; set; }
    }
}