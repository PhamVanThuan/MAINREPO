using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class ClientCannotBeAnExistingApplicantRule : IDomainRule<AddLeadApplicantToApplicationCommand>
    {
        private IApplicantDataManager applicantDataManager;

        public ClientCannotBeAnExistingApplicantRule(IApplicantDataManager applicantDataManager)
        {
            this.applicantDataManager = applicantDataManager;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, AddLeadApplicantToApplicationCommand ruleModel)
        {
            var clientRoleExists = this.applicantDataManager.CheckClientIsAnApplicantOnTheApplication(ruleModel.ClientKey, ruleModel.ApplicationNumber);
            if (clientRoleExists)
            {
                messages.AddMessage(new SystemMessage("The client provided is already an active applicant on this application.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}