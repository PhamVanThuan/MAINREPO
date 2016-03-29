using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.AddressDomain.Specs.Commands
{
    class When_creating_a_valid_LinkStreetAddressAsPostalAddressToClientCommand
    {
        private static LinkStreetAddressAsPostalAddressToClientCommand command;
        private static int clientKey;
        private static StreetAddressModel streetAddressModel;
        private static Guid clientAddressGuid;

        private Establish context = () =>
        {
            clientAddressGuid = CombGuid.Instance.Generate();
            streetAddressModel = new StreetAddressModel("", "", "", "7", "Maryland Avenue", "Durban North", "Durban", "Kwazulu-Natal", "4051");
            clientKey = 3;
        };
        Because of = () =>
        {
            command = new LinkStreetAddressAsPostalAddressToClientCommand(streetAddressModel, clientKey, clientAddressGuid);
        };

        It should_contain_a_valid_clientKey = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresClient));
        };
    }
}
