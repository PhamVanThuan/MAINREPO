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
    public class ApplicantContactDetailsValidation : IDomainRule<Applicant>
    {
        private IRuleHelper ruleHelper = new RuleHelper();

        public void ExecuteRule(ISystemMessageCollection messages, Applicant applicant)
        {
            if (String.IsNullOrWhiteSpace(applicant.EmailAddress)
                && String.IsNullOrWhiteSpace(applicant.Cellphone)
                && (String.IsNullOrWhiteSpace(applicant.WorkPhoneCode) || String.IsNullOrWhiteSpace(applicant.WorkPhone))
                && (String.IsNullOrWhiteSpace(applicant.HomePhoneCode) || String.IsNullOrWhiteSpace(applicant.HomePhone))
                )
            {
                string applicantName = ruleHelper.GetApplicantNameForErrorMessage(applicant);

                messages.AddMessage(new SystemMessage(applicantName + "At least one contact detail (Email, Home, Work or Cell Number) is required.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}