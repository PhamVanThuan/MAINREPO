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
    class When_creating_a_valid_LinkDomiciliumAddressToApplicantCommand
    {
        private static LinkDomiciliumAddressToApplicantCommand command;

        private Establish context = () =>
        {
        };
        Because of = () =>
        {
            command = new LinkDomiciliumAddressToApplicantCommand(null);
        };

        It should_contain_a_valid_client = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresClient));
        };

        It should_contain_a_valid_open_application = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresOpenApplication));
        };
    }
}
