using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.AssessmentCanContainOnlyOneOfEachAffordabilityType
{
    public class when_the_is_more_than_one_of_the_same_type : WithFakes
    {
        private static AssessmentCanContainOnlyOneOfEachAffordabilityTypeRule rule;
        private static ApplicantAffordabilityModel model;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            var clientAffordabilityAssessment = new List<AffordabilityTypeModel> { 
                new AffordabilityTypeModel(AffordabilityType.BasicSalary, 65000, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.BasicSalary, 25000, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.Childsupport, 11000, string.Empty),

            };
            model = new ApplicantAffordabilityModel(clientAffordabilityAssessment, 1234, 5678);
            rule = new AssessmentCanContainOnlyOneOfEachAffordabilityTypeRule();            
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_return_an_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("An applicant's affordability assessment can only contain one of each affordability type.");
        };
    }
}