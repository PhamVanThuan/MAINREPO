using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class QuickCashFeesDataModel :  IDataModel
    {
        public QuickCashFeesDataModel(double feeRange, double? fee, double? feePercentage)
        {
            this.FeeRange = feeRange;
            this.Fee = fee;
            this.FeePercentage = feePercentage;
		
        }		

        public double FeeRange { get; set; }

        public double? Fee { get; set; }

        public double? FeePercentage { get; set; }
    }
}