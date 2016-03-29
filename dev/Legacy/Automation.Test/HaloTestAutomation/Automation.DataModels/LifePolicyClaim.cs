using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class LifePolicyClaim
    {
        public ClaimTypeEnum ClaimTypeKey { get; set; }

        public ClaimStatusEnum ClaimStatusKey { get; set; }

        public int FinancialServiceKey { get; set; }

        public DateTime ClaimDate { get; set; }

        public int LifePolicyClaimKey { get; set; }
    }
}