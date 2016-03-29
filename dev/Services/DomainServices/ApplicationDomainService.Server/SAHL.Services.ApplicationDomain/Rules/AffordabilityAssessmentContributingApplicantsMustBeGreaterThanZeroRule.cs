using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class AffordabilityAssessmentContributingApplicantsMustBeGreaterThanZeroRule : IDomainRule<AffordabilityAssessmentModel>
    {
        public void ExecuteRule(ISystemMessageCollection messages, AffordabilityAssessmentModel ruleModel)
        {
            if (ruleModel.NumberOfContributingApplicants <= 0)
            {
                messages.AddMessage(new SystemMessage("The number of contributing applicants must be greater than 0.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}