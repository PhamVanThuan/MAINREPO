using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ValuationOutbuildingDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ValuationOutbuildingDataModel(int valuationKey, int? valuationRoofTypeKey, double? extent, double? rate)
        {
            this.ValuationKey = valuationKey;
            this.ValuationRoofTypeKey = valuationRoofTypeKey;
            this.Extent = extent;
            this.Rate = rate;
		
        }
		[JsonConstructor]
        public ValuationOutbuildingDataModel(int valuationOutbuildingKey, int valuationKey, int? valuationRoofTypeKey, double? extent, double? rate)
        {
            this.ValuationOutbuildingKey = valuationOutbuildingKey;
            this.ValuationKey = valuationKey;
            this.ValuationRoofTypeKey = valuationRoofTypeKey;
            this.Extent = extent;
            this.Rate = rate;
		
        }		

        public int ValuationOutbuildingKey { get; set; }

        public int ValuationKey { get; set; }

        public int? ValuationRoofTypeKey { get; set; }

        public double? Extent { get; set; }

        public double? Rate { get; set; }

        public void SetKey(int key)
        {
            this.ValuationOutbuildingKey =  key;
        }
    }
}