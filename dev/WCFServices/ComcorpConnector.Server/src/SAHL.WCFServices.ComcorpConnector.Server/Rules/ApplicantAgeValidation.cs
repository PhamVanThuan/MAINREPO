using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SAHL.WCFServices.ComcorpConnector.Server.Rules
{
    public class ApplicantAgeValidation : IDomainRule<Applicant>
    {
        private IValidationUtils validationUtils;
        private IRuleHelper ruleHelper = new RuleHelper();
        public ApplicantAgeValidation(IValidationUtils validationUtils)
        {
            this.validationUtils = validationUtils;
        }
        public void ExecuteRule(ISystemMessageCollection messages, Applicant applicant)
        {
            int age = validationUtils.GetAgeFromDateOfBirth(applicant.DateOfBirth);
            string applicantName = ruleHelper.GetApplicantNameForErrorMessage(applicant);

            if (age <= 18)
            {
                messages.AddMessage(new SystemMessage(applicantName + "Applicant should be older than 18.", SystemMessageSeverityEnum.Error));
            }

            if (age > 65)
            {
                messages.AddMessage(new SystemMessage(applicantName + "Applicant should be younger than 65.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}