using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.Interfaces.Application.Specs.Commands
{
    internal class When_creating_a_valid_AmendAffordabilityAssessmentIncomeContributorsCommand
    {
        private static AmendAffordabilityAssessmentIncomeContributorsCommand command;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            command = new AmendAffordabilityAssessmentIncomeContributorsCommand(Param.IsAny<AffordabilityAssessmentModel>());
        };

        private It should_implement_the_application_domain_command = () =>
        {
            command.ShouldBeAssignableTo(typeof(IApplicationDomainCommand));
        };
    }
}