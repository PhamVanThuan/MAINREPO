using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;

namespace SAHL.Services.Interfaces.Application.Specs.Commands
{
    internal class When_creating_a_valid_ConfirmApplicationAffordabilityAssessmentsCommand
    {
        private static ConfirmApplicationAffordabilityAssessmentsCommand command;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            command = new ConfirmApplicationAffordabilityAssessmentsCommand(Param.IsAny<int>());
        };

        private It should_implement_the_application_domain_command = () =>
        {
            command.ShouldBeAssignableTo(typeof(IApplicationDomainCommand));
        };
    }
}