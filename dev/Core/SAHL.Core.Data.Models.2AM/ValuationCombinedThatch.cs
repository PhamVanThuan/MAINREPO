using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ValuationCombinedThatchDataModel :  IDataModel
    {
        public ValuationCombinedThatchDataModel(int valuationKey, double? value)
        {
            this.ValuationKey = valuationKey;
            this.Value = value;
		
        }		

        public int ValuationKey { get; set; }

        public double? Value { get; set; }
    }
}