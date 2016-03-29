using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class AssessmentCanContainOnlyOneOfEachAffordabilityTypeRule : IDomainRule<ApplicantAffordabilityModel>
    {
        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, ApplicantAffordabilityModel ruleModel)
        {
            if (ruleModel.ClientAffordabilityAssessment != null)
            {
                var query = ruleModel.ClientAffordabilityAssessment.GroupBy(x => x.AffordabilityType).Where(g => g.Count() > 1).Select(y => y);
                if (query.FirstOrDefault() != null)
                {
                    messages.AddMessage(new SystemMessage("An applicant's affordability assessment can only contain one of each affordability type.", SystemMessageSeverityEnum.Error));
                }
            }
        }
    }
}