using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers.AssetLiability;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddFixedLongTermInvestmentLiabilityToClient
{
    public class when_adding_a_valid_fixed_long_term_investment_liability : WithCoreFakes
    {
        private static AddFixedLongTermInvestmentLiabilityToClientCommand command;
        private static AddFixedLongTermInvestmentLiabilityToClientCommandHandler handler;
        private static FixedLongTermInvestmentLiabilityModel model;
        private static IAssetLiabilityDataManager assetLiabilityDataManager;
        private static int assetLiabilityKey, clientKey, clientAssetLiabilityKey;
        private static IUnitOfWorkFactory uowFactory;

        private Establish context = () =>
        {
            assetLiabilityDataManager = An<IAssetLiabilityDataManager>();
            uowFactory = An<IUnitOfWorkFactory>();

            clientKey = 123456;
            model = new FixedLongTermInvestmentLiabilityModel("Test", 1);
            command = new AddFixedLongTermInvestmentLiabilityToClientCommand(model, clientKey);
            handler = new AddFixedLongTermInvestmentLiabilityToClientCommandHandler(assetLiabilityDataManager, eventRaiser, uowFactory);

            assetLiabilityKey = 1234;
            assetLiabilityDataManager.WhenToldTo(x => x.SaveFixedLongTermInvestmentLiability(Param.IsAny<FixedLongTermInvestmentLiabilityModel>())).Return(assetLiabilityKey);

            clientAssetLiabilityKey = 54321;
            assetLiabilityDataManager.WhenToldTo(x => x.LinkAssetLiabilityToClient(Param.IsAny<int>(), Param.IsAny<int>())).Return(clientAssetLiabilityKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_add_the_fixed_long_term_investment_liability = () =>
        {
            assetLiabilityDataManager.WasToldTo(x => x.SaveFixedLongTermInvestmentLiability(Arg.Is<FixedLongTermInvestmentLiabilityModel>(
                y => y.CompanyName == model.CompanyName && y.LiabilityValue == model.LiabilityValue)));
        };

        private It should_link_the_fixed_long_term_investment_liability_to_client = () =>
        {
            assetLiabilityDataManager.WasToldTo(x => x.LinkAssetLiabilityToClient(clientKey, assetLiabilityKey));
        };

        private It should_raise_a_long_term_investment_liability_added_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<FixedLongTermInvestmentLiabilityAddedToClientEvent>(
               y => y.CompanyName == command.FixedLongTermInvestmentLiabilityModel.CompanyName &&
                y.LiabilityValue == command.FixedLongTermInvestmentLiabilityModel.LiabilityValue),
                 clientAssetLiabilityKey, (int)GenericKeyType.LegalEntityAssetLiability, Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}