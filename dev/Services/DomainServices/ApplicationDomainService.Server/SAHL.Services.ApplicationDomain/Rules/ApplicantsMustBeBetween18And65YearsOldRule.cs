using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class ApplicantsMustBeBetween18And65YearsOldRule : IDomainRule<AddLeadApplicantToApplicationCommand>
    {
        private IApplicantDataManager applicantDataManager;
        private IValidationUtils validationUtils;

        public ApplicantsMustBeBetween18And65YearsOldRule(IApplicantDataManager applicantDataManager, IValidationUtils validationUtils)
        {
            this.applicantDataManager = applicantDataManager;
            this.validationUtils = validationUtils;
        }

        public void ExecuteRule(ISystemMessageCollection messages, AddLeadApplicantToApplicationCommand command)
        {
            DateTime? dateOfBirth = applicantDataManager.GetClientDateOfBirth(command.ClientKey);

            if (!dateOfBirth.HasValue)
            {
                messages.AddMessage(new SystemMessage("Applicant age could not be determined.", SystemMessageSeverityEnum.Error));
            }
            else
            {
                int age = validationUtils.GetAgeFromDateOfBirth(dateOfBirth.Value);

                if (age < 18)
                {
                    messages.AddMessage(new SystemMessage("An applicant on a mortgage loan application cannot be younger than 18 years of age.", SystemMessageSeverityEnum.Error));
                }

                if (age > 65)
                {
                    messages.AddMessage(new SystemMessage("An applicant on a mortgage loan application cannot be older than 65 years of age.", SystemMessageSeverityEnum.Error));
                }
            }
        }         
    }
}