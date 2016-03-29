using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Affordability;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class AffordabilityDescriptionRequiredRule : IDomainRule<ApplicantAffordabilityModel>
    {
        private IAffordabilityDataManager affordabilityDataManager;
        public AffordabilityDescriptionRequiredRule(IAffordabilityDataManager affordabilityDataManager)
        {
            this.affordabilityDataManager = affordabilityDataManager;
        }
        public void ExecuteRule(ISystemMessageCollection messages, ApplicantAffordabilityModel ruleModel)
        {
            //determine IsDescriptionRequired given the AffordabilityType
            foreach (var affordability in ruleModel.ClientAffordabilityAssessment)
            {
                var isDescriptionRequired = affordabilityDataManager.IsDescriptionRequired(affordability.AffordabilityType);
                if (isDescriptionRequired && string.IsNullOrWhiteSpace(affordability.Description))
                {
                    messages.AddMessage(new SystemMessage(
                        string.Format("An Affordability description is required for the '{0}' affordability type.", affordability.AffordabilityType), SystemMessageSeverityEnum.Error));
                }
            }
        }
    }
}