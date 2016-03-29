using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ValuationCottageDataModel :  IDataModel
    {
        public ValuationCottageDataModel(int valuationKey, int? valuationRoofTypeKey, double? extent, double? rate)
        {
            this.ValuationKey = valuationKey;
            this.ValuationRoofTypeKey = valuationRoofTypeKey;
            this.Extent = extent;
            this.Rate = rate;
		
        }		

        public int ValuationKey { get; set; }

        public int? ValuationRoofTypeKey { get; set; }

        public double? Extent { get; set; }

        public double? Rate { get; set; }
    }
}