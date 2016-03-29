using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Commands
{
    public class when_adding_other_asset_to_client : WithFakes
    {
        private static AddOtherAssetToClientCommand command;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            command = new AddOtherAssetToClientCommand(1111, new OtherAssetModel("Other Asset", 1d, 1d));
        };

        private It should_check_client_exists = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresClient));
        };
    }
}