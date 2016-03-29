using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Affordability;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class ApplicantCannotHaveAnExistingAffordabilityAssessmentRule : IDomainRule<ApplicantAffordabilityModel>
    {
        private IAffordabilityDataManager affordabilityDataManager;

        public ApplicantCannotHaveAnExistingAffordabilityAssessmentRule(IAffordabilityDataManager affordabilityDataManager)
        {
            this.affordabilityDataManager = affordabilityDataManager;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, ApplicantAffordabilityModel ruleModel)
        {
            var existingApplicantAffordabilityAssessment = this.affordabilityDataManager.GetAffordabilityAssessment(ruleModel.ClientKey, ruleModel.ApplicationNumber);
            if (existingApplicantAffordabilityAssessment.Any())
            {
                messages.AddMessage(new SystemMessage(
                    string.Format("An affordability assessment already exists for Client: {0} on ApplicationNumber: {1}", 
                    ruleModel.ClientKey, ruleModel.ApplicationNumber), SystemMessageSeverityEnum.Error));
            }

        }
    }
}