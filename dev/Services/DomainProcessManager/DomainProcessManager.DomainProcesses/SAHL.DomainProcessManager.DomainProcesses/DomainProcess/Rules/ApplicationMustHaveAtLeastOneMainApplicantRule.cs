using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.Models;
using System;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.Rules
{
    public class ApplicationMustHaveAtLeastOneMainApplicantRule : IDomainRule<ApplicationCreationModel>
    {
        public void ExecuteRule(ISystemMessageCollection messages, ApplicationCreationModel ruleModel)
        {
            var hasOneMainApplicant = ruleModel.Applicants != null && ruleModel.Applicants.Count() != 0
                && ruleModel.Applicants.Any(x => x.ApplicantRoleType == SAHL.Core.BusinessModel.Enums.LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant);
            if (!hasOneMainApplicant || ruleModel.Applicants == null || ruleModel.Applicants.Count() == 0)
            {
                messages.AddMessage(new SystemMessage("An application must have at least one main applicant.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}