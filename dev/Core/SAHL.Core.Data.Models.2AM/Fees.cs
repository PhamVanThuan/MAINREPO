using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class FeesDataModel :  IDataModel
    {
        public FeesDataModel(int feeRange, double? feeNaturalTransferDuty, double? feeNaturalConveyancing, double? feeNaturalVAT, double? feeLegalTransferDuty, double? feeLegalConveyancing, double? feeLegalVAT, double? feeBondStamps, double? feeBondConveyancing, double? feeBondVAT, double? feeAdmin, double? feeValuation, double? feeCancelDuty, double? feeCancelConveyancing, double? feeCancelVAT, double? feeFlexiSwitch, double? feeRCSBondConveyancing, double? feeRCSBondVAT, double? feeDeedsOffice, double? feeRCSBondPreparation, double? feeBondConveyancing80Pct, double? feeBondVAT80Pct, double? feeBondConveyancingNoFICA, double? feeBondNoFICAVAT)
        {
            this.FeeRange = feeRange;
            this.FeeNaturalTransferDuty = feeNaturalTransferDuty;
            this.FeeNaturalConveyancing = feeNaturalConveyancing;
            this.FeeNaturalVAT = feeNaturalVAT;
            this.FeeLegalTransferDuty = feeLegalTransferDuty;
            this.FeeLegalConveyancing = feeLegalConveyancing;
            this.FeeLegalVAT = feeLegalVAT;
            this.FeeBondStamps = feeBondStamps;
            this.FeeBondConveyancing = feeBondConveyancing;
            this.FeeBondVAT = feeBondVAT;
            this.FeeAdmin = feeAdmin;
            this.FeeValuation = feeValuation;
            this.FeeCancelDuty = feeCancelDuty;
            this.FeeCancelConveyancing = feeCancelConveyancing;
            this.FeeCancelVAT = feeCancelVAT;
            this.FeeFlexiSwitch = feeFlexiSwitch;
            this.FeeRCSBondConveyancing = feeRCSBondConveyancing;
            this.FeeRCSBondVAT = feeRCSBondVAT;
            this.FeeDeedsOffice = feeDeedsOffice;
            this.FeeRCSBondPreparation = feeRCSBondPreparation;
            this.FeeBondConveyancing80Pct = feeBondConveyancing80Pct;
            this.FeeBondVAT80Pct = feeBondVAT80Pct;
            this.FeeBondConveyancingNoFICA = feeBondConveyancingNoFICA;
            this.FeeBondNoFICAVAT = feeBondNoFICAVAT;
		
        }		

        public int FeeRange { get; set; }

        public double? FeeNaturalTransferDuty { get; set; }

        public double? FeeNaturalConveyancing { get; set; }

        public double? FeeNaturalVAT { get; set; }

        public double? FeeLegalTransferDuty { get; set; }

        public double? FeeLegalConveyancing { get; set; }

        public double? FeeLegalVAT { get; set; }

        public double? FeeBondStamps { get; set; }

        public double? FeeBondConveyancing { get; set; }

        public double? FeeBondVAT { get; set; }

        public double? FeeAdmin { get; set; }

        public double? FeeValuation { get; set; }

        public double? FeeCancelDuty { get; set; }

        public double? FeeCancelConveyancing { get; set; }

        public double? FeeCancelVAT { get; set; }

        public double? FeeFlexiSwitch { get; set; }

        public double? FeeRCSBondConveyancing { get; set; }

        public double? FeeRCSBondVAT { get; set; }

        public double? FeeDeedsOffice { get; set; }

        public double? FeeRCSBondPreparation { get; set; }

        public double? FeeBondConveyancing80Pct { get; set; }

        public double? FeeBondVAT80Pct { get; set; }

        public double? FeeBondConveyancingNoFICA { get; set; }

        public double? FeeBondNoFICAVAT { get; set; }
    }
}