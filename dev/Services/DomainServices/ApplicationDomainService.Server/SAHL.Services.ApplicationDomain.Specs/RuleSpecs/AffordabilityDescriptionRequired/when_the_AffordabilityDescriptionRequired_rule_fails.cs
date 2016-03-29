using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Affordability;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.ApplicationDomain;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using SAHL.Core.Testing;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.AffordabilityDescriptionRequired
{
    public class when_the_AffordabilityDescriptionRequired_rule_fails : WithCoreFakes
    {
        private static Rules.AffordabilityDescriptionRequiredRule rule;
        private static ApplicantAffordabilityModel applicantAffordabilityModel;
        private static IAffordabilityDataManager affordabilityDataManager;
        private static AffordabilityType typeThatRequiresDescription;

        private Establish context = () =>
        {
            typeThatRequiresDescription = AffordabilityType.OtherInstalments;
            messages = new SystemMessageCollection();
            affordabilityDataManager =  An<IAffordabilityDataManager>();
            applicantAffordabilityModel = new ApplicantAffordabilityModel(new List<AffordabilityTypeModel> { 
                new AffordabilityTypeModel(AffordabilityType.BasicSalary, 5000000, string.Empty),
                new AffordabilityTypeModel(typeThatRequiresDescription, 25000, string.Empty) }, 12322, 43434);
            rule = new Rules.AffordabilityDescriptionRequiredRule(affordabilityDataManager);

            affordabilityDataManager.WhenToldTo(x => x.IsDescriptionRequired(typeThatRequiresDescription)).Return(true);
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
            messages.ErrorMessages().ShouldContain(x => x.Message ==
                string.Format("An Affordability description is required for the '{0}' affordability type.", typeThatRequiresDescription));
        };
    }
}