using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class ApplicantCannotBeAnExistingIncomeContributorRule : IDomainRule<ApplicantRoleModel>
    {
        private IApplicantManager applicantManager;

        public ApplicantCannotBeAnExistingIncomeContributorRule(IApplicantManager applicantManager)
        {
            this.applicantManager = applicantManager;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, ApplicantRoleModel ruleModel)
        {
            if (applicantManager.IsApplicantAnIncomeContributor(ruleModel.ApplicationRoleKey))
            {
                messages.AddMessage(new SystemMessage("Applicant is already an income contributor", SystemMessageSeverityEnum.Error));
            }
        }
    }
}