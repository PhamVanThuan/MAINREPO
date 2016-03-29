using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RiskMatrixRangeDataModel :  IDataModel
    {
        public RiskMatrixRangeDataModel(int riskMatrixRangeKey, double? min, double? max, string designation)
        {
            this.RiskMatrixRangeKey = riskMatrixRangeKey;
            this.Min = min;
            this.Max = max;
            this.Designation = designation;
		
        }		

        public int RiskMatrixRangeKey { get; set; }

        public double? Min { get; set; }

        public double? Max { get; set; }

        public string Designation { get; set; }
    }
}