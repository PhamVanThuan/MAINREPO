using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class SnapshotRateOverride
    {
        public int SnapShotRateOverrideKey { get; set; }

        public int SnapShotFinancialServiceKey { get; set; }

        public int AccountKey { get; set; }

        public int RateOverrideKey { get; set; }

        public int FinancialServiceKey { get; set; }

        public string RateOverrideTypeDescription { get; set; }

        public RateOverrideTypeEnum RateOverrideTypeKey { get; set; }

        public DateTime? FromDate { get; set; }

        public int Term { get; set; }

        public double CapRate { get; set; }

        public double FloorRate { get; set; }

        public double FixedRate { get; set; }

        public double Discount { get; set; }

        public int GeneralStatusKey { get; set; }

        public int TradeKey { get; set; }

        public DateTime? CancellationDate { get; set; }

        public int CancellationReasonKey { get; set; }

        public double CapBalance { get; set; }

        public double Amount { get; set; }

        public int CAPPaymentOptionKey { get; set; }

        public DateTime? EndDate { get; set; }
    }
}