using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Commands
{
    public class when_adding_investment_asset_to_client : WithFakes
    {
        private static AddInvestmentAssetToClientCommand command;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            command = new AddInvestmentAssetToClientCommand(1111, new InvestmentAssetModel(AssetInvestmentType.ListedInvestments, "Other Asset", 1d));
        };

        private It should_check_client_exists = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresClient));
        };
    }
}