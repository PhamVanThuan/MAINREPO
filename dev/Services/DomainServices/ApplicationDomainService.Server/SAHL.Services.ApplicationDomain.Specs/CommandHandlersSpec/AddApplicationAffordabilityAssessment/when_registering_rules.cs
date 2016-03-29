using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM.Managers.ADUser;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplicationAffordabilityAssessment
{
    public class when_registering_rules : WithCoreFakes
    {
        private static AddApplicationAffordabilityAssessmentCommand command;
        private static AddApplicationAffordabilityAssessmentCommandHandler handler;

        private static AffordabilityAssessmentModel affordabilityAssessmentModel;

        private static IDomainRuleManager<AffordabilityAssessmentModel> domainRuleManager;
        private static IADUserManager aduserManager;
        private static IAffordabilityAssessmentManager affordabilityAssessmentManager;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<AffordabilityAssessmentModel>>();
            aduserManager = An<IADUserManager>();
            affordabilityAssessmentManager = An<IAffordabilityAssessmentManager>();

            affordabilityAssessmentModel = new AffordabilityAssessmentModel();
            command = new AddApplicationAffordabilityAssessmentCommand(affordabilityAssessmentModel);
        };

        private Because of = () =>
        {
            handler = new AddApplicationAffordabilityAssessmentCommandHandler(domainRuleManager, aduserManager, affordabilityAssessmentManager, eventRaiser);
        };

        private It should_register_income_contributor_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<AffordabilityAssessmentRequiresAtLeastOneIncomeContributorRule>()));
        };

        private It should_register_contributing_applicants_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<AffordabilityAssessmentContributingApplicantsMustBeGreaterThanZeroRule>()));
        };
    }
}