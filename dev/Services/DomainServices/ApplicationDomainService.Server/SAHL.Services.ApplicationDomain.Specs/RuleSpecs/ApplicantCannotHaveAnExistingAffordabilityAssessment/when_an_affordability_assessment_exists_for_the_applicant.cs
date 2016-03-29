using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Affordability;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.ApplicantCannotHaveAnExistingAffordabilityAssessment
{
    public class when_an_affordability_assessment_exists_for_the_applicant : WithFakes
    {
        private static ApplicantCannotHaveAnExistingAffordabilityAssessmentRule rule;
        private static IAffordabilityDataManager affordabilityDataManager;
        private static ISystemMessageCollection messages;
        private static ApplicantAffordabilityModel model;
        private static IEnumerable<LegalEntityAffordabilityDataModel> existingAffordabilityAssessment;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            existingAffordabilityAssessment = new LegalEntityAffordabilityDataModel[] { new LegalEntityAffordabilityDataModel(1, 1, 10000, "Test", 1234567) };
            affordabilityDataManager = An<IAffordabilityDataManager>();
            var clientAffordabilityAssessment = new List<AffordabilityTypeModel> {
                new AffordabilityTypeModel(AffordabilityType.BasicSalary, 65000, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.BasicSalary, 25000, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.Childsupport, 11000, string.Empty),
            };
            model = new ApplicantAffordabilityModel(clientAffordabilityAssessment, 1234, 5678);
            affordabilityDataManager.WhenToldTo(x => x.GetAffordabilityAssessment(model.ClientKey, model.ApplicationNumber)).Return(existingAffordabilityAssessment);
            rule = new ApplicantCannotHaveAnExistingAffordabilityAssessmentRule(affordabilityDataManager);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_return_an_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual(
                string.Format("An affordability assessment already exists for Client: {0} on ApplicationNumber: {1}", model.ClientKey, model.ApplicationNumber)
                );
        };
    }
}