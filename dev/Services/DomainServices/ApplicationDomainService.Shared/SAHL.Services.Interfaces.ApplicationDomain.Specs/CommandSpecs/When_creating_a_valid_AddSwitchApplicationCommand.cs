using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;

namespace SAHL.Services.Interfaces.Application.Specs.Commands
{
    internal class When_creating_a_valid_AddSwitchApplicationCommand
    {
        private static AddSwitchApplicationCommand command;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            command = new AddSwitchApplicationCommand(Param.IsAny<SwitchApplicationModel>(), Param.IsAny<Guid>());
        };

        private It should_implement_the_application_domain_command = () =>
        {
            command.ShouldBeAssignableTo(typeof(IApplicationDomainCommand));
        };
    }
}