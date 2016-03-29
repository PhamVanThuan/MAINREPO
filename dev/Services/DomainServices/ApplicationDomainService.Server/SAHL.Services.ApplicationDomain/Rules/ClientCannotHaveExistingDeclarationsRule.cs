using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class ClientCannotHaveExistingDeclarationsRule : IDomainRule<ApplicantDeclarationsModel>
    {
        private IApplicantDataManager applicantDataManager;

        public ClientCannotHaveExistingDeclarationsRule(IApplicantDataManager applicantDataManager)
        {
            this.applicantDataManager = applicantDataManager;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, ApplicantDeclarationsModel ruleModel)
        {
            var applicantDeclarations = this.applicantDataManager.GetApplicantDeclarations(ruleModel.ApplicationNumber, ruleModel.ClientKey);
            if (applicantDeclarations.Count() > 0)
            {
                messages.AddMessage(new SystemMessage("Applicant has already provided declarations for this application.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}