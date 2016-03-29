using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Affordability;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using NSubstitute;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplicantAffordability
{
    public class when_registering_applicant_affordabilities_rules: WithCoreFakes
    {
        private static AddApplicantAffordabilitiesCommandHandler handler;
        private static IDomainRuleManager<ApplicantAffordabilityModel> domainRuleManager;
        private static IAffordabilityDataManager affordabilityDataManager;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<ApplicantAffordabilityModel>>();
            affordabilityDataManager = An<IAffordabilityDataManager>();
        };

        private Because of = () =>
        {
            handler = new AddApplicantAffordabilitiesCommandHandler(domainRuleManager, Param.IsAny<IApplicantDataManager>(), eventRaiser, unitOfWorkFactory, affordabilityDataManager);
        };

        
        private It should_ensure_the_AffordabilityDescriptionRequired_rule = () =>
        {
            domainRuleManager.Received().RegisterRule(Arg.Any<AffordabilityDescriptionRequiredRule>());
        };

        private It should_register_the_AssessmentCanContainOnlyOneOfEachAffordabilityType_Rule = () =>
        {
            domainRuleManager.Received().RegisterRule(Arg.Any<AssessmentCanContainOnlyOneOfEachAffordabilityTypeRule>());
        };

        private It should_register_the_ApplicantCannotHaveAnExistingAffordabilityAssessment_rule = () =>
        {
            domainRuleManager.Received().RegisterRule(Arg.Any<ApplicantCannotHaveAnExistingAffordabilityAssessmentRule>());
        };
    }
}