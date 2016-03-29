using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Server.Rules
{
    public class ApplicantPassportNumberRequiredRule : IDomainRule<Applicant>
    {
        private IValidationUtils validationUtils;
        private IRuleHelper ruleHelper = new RuleHelper();

        public ApplicantPassportNumberRequiredRule(IValidationUtils validationUtils)
        {
            this.validationUtils = validationUtils;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, Applicant ruleModel)
        {
            if (ruleModel.SAHLSACitizenType.StartsWith("Non - Resident") || ruleModel.SAHLSACitizenType.Equals("Foreigner", StringComparison.OrdinalIgnoreCase))
            {
                if (String.IsNullOrWhiteSpace(ruleModel.PassportNo))
                {
                    string applicantName = ruleHelper.GetApplicantNameForErrorMessage(ruleModel);
                    messages.AddMessage(new SystemMessage(applicantName + "Applicant must have a Passport Number.", SystemMessageSeverityEnum.Error));
                }
            }
        }
    }
}