using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class SnapshotAccount
    {
        public int SnapShotAccountKey { get; set; }

        public int AccountKey { get; set; }

        public int RemainingInstallments { get; set; }

        public ProductEnum ProductKey { get; set; }

        public int ValuationKey { get; set; }

        public DateTime InsertDate { get; set; }

        public int DebtCounsellingKey { get; set; }

        public double HOCPremium { get; set; }

        public double LifePremium { get; set; }

        public SnapshotFinancialService SnapshotFinancialService { get; set; }
    }
}