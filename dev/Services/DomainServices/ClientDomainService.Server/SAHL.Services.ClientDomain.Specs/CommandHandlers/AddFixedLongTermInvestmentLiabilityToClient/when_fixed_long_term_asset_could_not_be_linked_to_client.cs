using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers.AssetLiability;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddFixedLongTermInvestmentLiabilityToClient
{
    public class when_fixed_long_term_asset_could_not_be_linked_to_client : WithCoreFakes
    {
        private static AddFixedLongTermInvestmentLiabilityToClientCommand command;
        private static AddFixedLongTermInvestmentLiabilityToClientCommandHandler handler;
        private static FixedLongTermInvestmentLiabilityModel model;
        private static IAssetLiabilityDataManager assetLiabilityDataManager;
        private static int assetLiabilityKey, clientKey, clientAssetLiabilityKey;

        private Establish context = () =>
        {
            assetLiabilityDataManager = An<IAssetLiabilityDataManager>();
            clientKey = 123456;
            model = new FixedLongTermInvestmentLiabilityModel("Test", 1);
            command = new AddFixedLongTermInvestmentLiabilityToClientCommand(model, clientKey);
            handler = new AddFixedLongTermInvestmentLiabilityToClientCommandHandler(assetLiabilityDataManager, eventRaiser, unitOfWorkFactory);

            assetLiabilityKey = 1234;
            assetLiabilityDataManager.WhenToldTo
                (x => x.SaveFixedLongTermInvestmentLiability(Param.IsAny<FixedLongTermInvestmentLiabilityModel>())).Return(assetLiabilityKey);

            clientAssetLiabilityKey = 0;
            assetLiabilityDataManager.WhenToldTo
                (x => x.LinkAssetLiabilityToClient(Param.IsAny<int>(), Param.IsAny<int>())).Return(clientAssetLiabilityKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_save_fixed_asset_liability = () =>
        {
            assetLiabilityDataManager.WasToldTo(x => x.SaveFixedLongTermInvestmentLiability(Arg.Is<FixedLongTermInvestmentLiabilityModel>(
                y => y.CompanyName == model.CompanyName && y.LiabilityValue == model.LiabilityValue)));
        };

        private It should_link_fixed_asset_liability_to_client = () =>
        {
            assetLiabilityDataManager.WasToldTo(x => x.LinkAssetLiabilityToClient(clientKey, assetLiabilityKey));
        };

        private It should_return_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("An error occurred when linking the fixed long term investment to the client.");
        };

        private It should_complete_the_unit_of_work = () =>
        {
            unitOfWork.WasToldTo(x => x.Complete());
        };
    }
}