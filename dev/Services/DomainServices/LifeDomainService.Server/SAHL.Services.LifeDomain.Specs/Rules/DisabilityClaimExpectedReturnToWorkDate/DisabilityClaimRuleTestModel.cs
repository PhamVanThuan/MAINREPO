using SAHL.Services.Interfaces.LifeDomain.Models;
using System;

namespace SAHL.Services.LifeDomain.Specs.RuleSpecs.DisabilityClaimExpectedReturnToWorkDate
{
    public class DisabilityClaimRuleTestModel : IDisabilityClaimRuleModel
    {
        public DisabilityClaimRuleTestModel(DateTime? expectedReturnToWorkDate, DateTime lastDateWorked)
        {
            this.ExpectedReturnToWorkDate = expectedReturnToWorkDate;
            this.LastDateWorked = lastDateWorked;
        }

        public DateTime? ExpectedReturnToWorkDate { get; protected set; }

        public DateTime LastDateWorked { get; protected set; }
    }
}