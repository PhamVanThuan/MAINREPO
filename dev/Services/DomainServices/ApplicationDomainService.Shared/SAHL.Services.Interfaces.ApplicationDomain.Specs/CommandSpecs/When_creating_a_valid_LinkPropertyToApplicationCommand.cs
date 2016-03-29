using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.Interfaces.Application.Specs.Commands
{
    internal class When_creating_a_valid_LinkPropertyToApplicationCommand
    {
        private static LinkPropertyToApplicationCommand command;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            command = new LinkPropertyToApplicationCommand(Param.IsAny<LinkPropertyToApplicationCommandModel>());
        };

        private It should_implement_the_application_domain_command = () =>
        {
            command.ShouldBeAssignableTo(typeof(IApplicationDomainCommand));
        };

        It should_contain_a_valid_open_application = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresOpenApplication));
        };

        It should_contain_a_valid_property = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresProperty));
        };
    }
}