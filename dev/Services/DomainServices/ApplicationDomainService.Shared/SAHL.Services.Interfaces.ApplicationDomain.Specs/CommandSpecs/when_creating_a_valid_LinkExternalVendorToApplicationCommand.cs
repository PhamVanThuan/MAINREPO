using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.ApplicationDomain.Specs.CommandSpecs
{
    public class when_creating_a_valid_LinkExternalVendorToApplicationCommand
    {
        private static LinkExternalVendorToApplicationCommand command;

        private Establish context = () =>
        {
        };

        Because of = () =>
        {
            command = new LinkExternalVendorToApplicationCommand(1111,OriginationSource.Comcorp, "VendorCode");
        };

        It should_contain_a_valid_open_application = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresOpenApplication));
        };
    }
}
