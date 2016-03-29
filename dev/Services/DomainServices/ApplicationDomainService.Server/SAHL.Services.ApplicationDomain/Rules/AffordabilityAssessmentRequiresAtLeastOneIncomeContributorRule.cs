using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class AffordabilityAssessmentRequiresAtLeastOneIncomeContributorRule : IDomainRule<AffordabilityAssessmentModel>
    {
        public void ExecuteRule(ISystemMessageCollection messages, AffordabilityAssessmentModel ruleModel)
        {
            if (!ruleModel.ContributingApplicantLegalEntities.Any())
            {
                messages.AddMessage(new SystemMessage("An affordability assessment requires at least one income contributor.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}