using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Commands
{
    public class when_adding_life_assurance_asset_to_client : WithFakes
    {
        private static AddLifeAssuranceAssetToClientCommand command;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            command = new AddLifeAssuranceAssetToClientCommand(1111, new LifeAssuranceAssetModel("Company name", 1d));
        };

        private It should_check_client_exists = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresClient));
        };
    }
}