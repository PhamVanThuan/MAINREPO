using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Identity;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Application.Specs.Commands
{
    class When_creating_a_valid_SetExternalOriginatorAttributeCommand
    {
        private static SetExternalOriginatorAttributeCommand command;

        private Establish context = () =>
        {
        };
        Because of = () =>
        {
            command = new SetExternalOriginatorAttributeCommand(1, OriginationSource.Comcorp);
        };

        It should_contain_a_valid_open_application = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresOpenApplication));
        };
    }
}
