using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class CapitecFeesDataModel :  IDataModel
    {
        public CapitecFeesDataModel(int feeRange, double? feeBondStamps, double? feeBondConveyancingNoFICA, double? feeBondNoFICAVAT, double? feeBondConveyancing80Pct, double? feeBondVAT80Pct, double? feeBondConveyancing, double? feeBondVAT, double? feeCancelDuty, double? feeCancelConveyancing, double? feeCancelVAT)
        {
            this.FeeRange = feeRange;
            this.FeeBondStamps = feeBondStamps;
            this.FeeBondConveyancingNoFICA = feeBondConveyancingNoFICA;
            this.FeeBondNoFICAVAT = feeBondNoFICAVAT;
            this.FeeBondConveyancing80Pct = feeBondConveyancing80Pct;
            this.FeeBondVAT80Pct = feeBondVAT80Pct;
            this.FeeBondConveyancing = feeBondConveyancing;
            this.FeeBondVAT = feeBondVAT;
            this.FeeCancelDuty = feeCancelDuty;
            this.FeeCancelConveyancing = feeCancelConveyancing;
            this.FeeCancelVAT = feeCancelVAT;
		
        }		

        public int FeeRange { get; set; }

        public double? FeeBondStamps { get; set; }

        public double? FeeBondConveyancingNoFICA { get; set; }

        public double? FeeBondNoFICAVAT { get; set; }

        public double? FeeBondConveyancing80Pct { get; set; }

        public double? FeeBondVAT80Pct { get; set; }

        public double? FeeBondConveyancing { get; set; }

        public double? FeeBondVAT { get; set; }

        public double? FeeCancelDuty { get; set; }

        public double? FeeCancelConveyancing { get; set; }

        public double? FeeCancelVAT { get; set; }
    }
}