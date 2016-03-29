using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Server.Rules;
using System;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Server
{
    public class ApplicantIDNumberRequiredRule : IDomainRule<Applicant>
    {
        private IValidationUtils validationUtils;
        private IRuleHelper ruleHelper = new RuleHelper();

        public ApplicantIDNumberRequiredRule(IValidationUtils validationUtils)
        {
            this.validationUtils = validationUtils;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, Applicant ruleModel)
        {
            if (ruleModel.SAHLSACitizenType.Equals("SA Citizen") || ruleModel.SAHLSACitizenType.Equals("SA Citizen – Non Resident"))
            {
                string applicantName = ruleHelper.GetApplicantNameForErrorMessage(ruleModel);
                if (String.IsNullOrWhiteSpace(ruleModel.IdentificationNo))
                {
                    messages.AddMessage(new SystemMessage(applicantName + "Applicant must have an ID Number.", SystemMessageSeverityEnum.Error));
                }
                else if (!validationUtils.ValidateIDNumber(ruleModel.IdentificationNo))
                {
                    messages.AddMessage(new SystemMessage(applicantName + "Applicant ID Number is invalid.", SystemMessageSeverityEnum.Error));
                }
            }
        }
    }
}