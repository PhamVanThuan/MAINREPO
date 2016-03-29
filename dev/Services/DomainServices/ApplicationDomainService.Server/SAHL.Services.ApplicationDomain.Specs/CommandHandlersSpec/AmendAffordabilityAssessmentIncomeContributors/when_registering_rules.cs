using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AmendAffordabilityAssessmentIncomeContributors
{
    public class when_registering_rules : WithCoreFakes
    {
        private static AmendAffordabilityAssessmentIncomeContributorsCommand command;
        private static AmendAffordabilityAssessmentIncomeContributorsCommandHandler handler;

        private static AffordabilityAssessmentModel affordabilityAssessmentModel;

        private static IDomainRuleManager<AffordabilityAssessmentModel> domainRuleManager;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<AffordabilityAssessmentModel>>();
            serviceCommandRouter = An<IServiceCommandRouter>();

            affordabilityAssessmentModel = new AffordabilityAssessmentModel();
            command = new AmendAffordabilityAssessmentIncomeContributorsCommand(affordabilityAssessmentModel);
        };

        private Because of = () =>
        {
            handler = new AmendAffordabilityAssessmentIncomeContributorsCommandHandler(domainRuleManager, serviceCommandRouter, eventRaiser);
        };

        private It should_register_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<AffordabilityAssessmentRequiresAtLeastOneIncomeContributorRule>()));
        };
    }
}