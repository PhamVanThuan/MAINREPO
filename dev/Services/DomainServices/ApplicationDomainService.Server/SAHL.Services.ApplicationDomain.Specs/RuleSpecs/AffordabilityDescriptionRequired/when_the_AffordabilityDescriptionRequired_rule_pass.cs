using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.Managers.Affordability;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.AffordabilityDescriptionRequired
{
    public class when_the_AffordabilityDescriptionRequired_rule_pass : WithCoreFakes
    {
        private static Rules.AffordabilityDescriptionRequiredRule rule;
        private static ApplicantAffordabilityModel applicantAffordabilityModel;
        private static IAffordabilityDataManager affordabilityDataManager;

        private Establish context = () =>
        {
            messages = new SystemMessageCollection();
            affordabilityDataManager = An<IAffordabilityDataManager>();
            applicantAffordabilityModel = new ApplicantAffordabilityModel(new List<AffordabilityTypeModel> { new AffordabilityTypeModel(AffordabilityType.BasicSalary, 5000000, "Salary") }, 
                12322, 43434);
            rule = new Rules.AffordabilityDescriptionRequiredRule(affordabilityDataManager);

            affordabilityDataManager.WhenToldTo(x => x.IsDescriptionRequired(applicantAffordabilityModel.ClientAffordabilityAssessment.First().AffordabilityType)).Return(true);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, applicantAffordabilityModel);
        };

        private It should_check_if_affordability_description_is_required = () =>
        {
            affordabilityDataManager.WasToldTo(x => x.IsDescriptionRequired(applicantAffordabilityModel.ClientAffordabilityAssessment.First().AffordabilityType));
        };

        private It should_return_messages = () =>
        {
            messages.ErrorMessages().ShouldNotContain(x => x.Message == "Affordability description is required.");
        };
    }
}