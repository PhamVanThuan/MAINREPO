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
    public class when_fixed_long_term_asset_failed_to_save : WithCoreFakes
    {
        private static AddFixedLongTermInvestmentLiabilityToClientCommand command;
        private static AddFixedLongTermInvestmentLiabilityToClientCommandHandler handler;
        private static FixedLongTermInvestmentLiabilityModel model;
        private static IAssetLiabilityDataManager assetLiabilityDataManager;
        private static int assetLiabilityKey, clientKey;

        private Establish context = () =>
        {
            assetLiabilityDataManager = An<IAssetLiabilityDataManager>();
            clientKey = 123456;
            model = new FixedLongTermInvestmentLiabilityModel("Test", 1);
            command = new AddFixedLongTermInvestmentLiabilityToClientCommand(model, clientKey);
            handler = new AddFixedLongTermInvestmentLiabilityToClientCommandHandler(assetLiabilityDataManager, eventRaiser, unitOfWorkFactory);
            assetLiabilityKey = 0;
            assetLiabilityDataManager.WhenToldTo(x => x.SaveFixedLongTermInvestmentLiability(Param.IsAny<FixedLongTermInvestmentLiabilityModel>())).Return(assetLiabilityKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_add_the_fixed_long_term_investment_liability = () =>
        {
            assetLiabilityDataManager.WasToldTo
                (x => x.SaveFixedLongTermInvestmentLiability(Arg.Is<FixedLongTermInvestmentLiabilityModel>(
                    y => y.CompanyName == model.CompanyName && y.LiabilityValue == model.LiabilityValue)));
        };

        private It should_return_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("An error occured when saving the fixed long term investment.");
        };

        private It should_not_link_asset_liability_to_client = () =>
        {
            assetLiabilityDataManager.WasNotToldTo(x => x.LinkAssetLiabilityToClient(clientKey, assetLiabilityKey));
        };

        private It should_complete_the_unit_of_work = () =>
        {
            unitOfWork.WasToldTo(x => x.Complete());
        };
    }
}