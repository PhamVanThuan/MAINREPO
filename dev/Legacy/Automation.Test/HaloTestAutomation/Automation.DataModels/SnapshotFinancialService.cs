using Common.Enums;

namespace Automation.DataModels
{
    public class SnapshotFinancialService
    {
        public int SnapShotFinancialServiceKey { get; set; }

        public int SnapShotAccountKey { get; set; }

        public int AccountKey { get; set; }

        public int FinancialServiceKey { get; set; }

        public FinancialServiceTypeEnum FinancialServiceTypeKey { get; set; }

        public int ResetConfigurationKey { get; set; }

        public double ActiveMarketRate { get; set; }

        public double Margin { get; set; }

        public double Installment { get; set; }

        public SnapshotRateOverride SnapshotRateOverride { get; set; }
    }
}