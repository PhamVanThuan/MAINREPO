using SAHL.Core.SystemMessages;
using SAHL.Workflow.Maps.Config;

namespace SAHL.Workflow.LifeClaims
{
    public interface ICreateDisabilityClaimDomainProcess : IWorkflowService
    {
        string GetDisabilityClaimInstanceSubject(ISystemMessageCollection messages, int disabilityClaimKey);

        bool CheckIfDisabilityClaimExclusionsExist(ISystemMessageCollection messages, int disabilityClaimKey);

        void DisabilityClaimSendManualApprovalLetter(ISystemMessageCollection messages, int disabilityClaimKey);
    }
}