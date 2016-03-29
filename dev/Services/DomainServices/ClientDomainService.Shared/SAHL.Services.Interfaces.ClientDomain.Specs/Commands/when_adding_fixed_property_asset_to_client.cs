using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Commands
{
    public class when_adding_fixed_property_asset_to_client : WithFakes
    {
        private static AddFixedPropertyAssetToClientCommand command;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            command = new AddFixedPropertyAssetToClientCommand(1111, new FixedPropertyAssetModel(DateTime.Now, 1, 1, 1));
        };

        private It should_check_client_exists = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresClient));
        };
    }
}