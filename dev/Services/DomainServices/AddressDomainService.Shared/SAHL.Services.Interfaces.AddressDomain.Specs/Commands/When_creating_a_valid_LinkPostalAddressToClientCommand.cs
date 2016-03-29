using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
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
    class When_creating_a_valid_LinkPostalAddressToClientCommand
    {
        private static LinkPostalAddressToClientCommand command;
        private static int clientKey;
        private static PostalAddressModel postalAddressModel;
        private static Guid clientAddressGuid;

        private Establish context = () =>
        {
            clientAddressGuid = CombGuid.Instance.Generate();
            postalAddressModel = new PostalAddressModel("1131", "", "Wandsbeck", "KZN", "Durban", "3629", AddressFormat.Box);
            clientKey = 3;
        };
        Because of = () =>
        {
            command = new LinkPostalAddressToClientCommand(postalAddressModel, clientKey, clientAddressGuid);
        };

        It should_contain_a_valid_clientKey = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresClient));
        };
    }
}
