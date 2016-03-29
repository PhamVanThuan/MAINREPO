using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Affordability;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.AffordabilityDescriptionRequired
{
    public class when_the_AffordabilityDescriptionRequired_rule_is_executed : WithFakes
    {
        private static SAHL.Services.ApplicationDomain.Rules.AffordabilityDescriptionRequiredRule rule;
        private static ISystemMessageCollection messages;
        private static ApplicantAffordabilityModel applicantAffordabilityModel;
        private static IAffordabilityDataManager affordabilityDataManager;

        private Establish context = () =>
        {
            messages = An<ISystemMessageCollection>();
            affordabilityDataManager = An<IAffordabilityDataManager>();
            applicantAffordabilityModel = new ApplicantAffordabilityModel(new List<AffordabilityTypeModel> { new AffordabilityTypeModel(AffordabilityType.BasicSalary, 5000000, "Salary") }, 
                12322, 43434);
            rule = new Rules.AffordabilityDescriptionRequiredRule(affordabilityDataManager);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, applicantAffordabilityModel);
        };

        private It should_check_if_the_affordability_type_requires_a_description = () =>
        {
            affordabilityDataManager.WasToldTo(x => x.IsDescriptionRequired(Param.IsAny<Core.BusinessModel.Enums.AffordabilityType>())).
                Times(applicantAffordabilityModel.ClientAffordabilityAssessment.Count());
        };
    }
}