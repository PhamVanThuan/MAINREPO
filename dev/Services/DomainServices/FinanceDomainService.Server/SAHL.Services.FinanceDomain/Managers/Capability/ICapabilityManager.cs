using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.Managers.Capability
{
    public interface ICapabilityManager
    {
        IEnumerable<ApprovalMandateRanges> GetCapabilityMandates();
    }
}
