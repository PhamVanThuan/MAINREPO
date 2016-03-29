using System;

namespace SAHL.Services.Interfaces.LifeDomain.Models
{
    public interface IDisabilityClaimRuleModel
    {
        DateTime? ExpectedReturnToWorkDate { get; }

        DateTime LastDateWorked { get; }
    }
}