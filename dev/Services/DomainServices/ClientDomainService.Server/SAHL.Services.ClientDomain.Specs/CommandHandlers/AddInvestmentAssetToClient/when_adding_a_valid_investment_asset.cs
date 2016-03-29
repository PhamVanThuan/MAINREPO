using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers.AssetLiability;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddInvestmentAssetToClient
{
    public class when_adding_a_valid_investment_asset : WithCoreFakes
    {
        private static AddInvestmentAssetToClientCommandHandler handler;
        private static AddInvestmentAssetToClientCommand command;
        private static int clientKey;
        private static InvestmentAssetModel investmentAsset;
        private static IAssetLiabilityDataManager assetLiabilityDataManager;
        private static int assetLiabilityKey, legalEntityAssetLiabilityKey;

        Establish context = () =>
        {
            assetLiabilityDataManager = An<IAssetLiabilityDataManager>();

            handler = new AddInvestmentAssetToClientCommandHandler(assetLiabilityDataManager, unitOfWorkFactory, eventRaiser);

            clientKey = 1111;
            investmentAsset = new InvestmentAssetModel(AssetInvestmentType.ListedInvestments, "Company name", 1d);
            command = new AddInvestmentAssetToClientCommand(clientKey, investmentAsset);

            assetLiabilityKey = 2222;
            assetLiabilityDataManager.WhenToldTo(x => x.SaveInvestmentAsset(Param.IsAny<InvestmentAssetModel>())).Return(assetLiabilityKey);

            legalEntityAssetLiabilityKey = 3333;
            assetLiabilityDataManager.WhenToldTo
                (x => x.LinkAssetLiabilityToClient(Param.IsAny<int>(), Param.IsAny<int>()))
                   .Return(legalEntityAssetLiabilityKey);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_begin_unit_of_work = () =>
        {
            unitOfWorkFactory.WasToldTo(x => x.Build());
        };

        private It should_add_other_asset = () =>
        {
            assetLiabilityDataManager.WasToldTo
                (x => x.SaveInvestmentAsset(Arg.Is<InvestmentAssetModel>(y => y.CompanyName == investmentAsset.CompanyName)));
        };

        private It should_link_fixed_property_asset_to_client = () =>
        {
            assetLiabilityDataManager.WasToldTo(x => x.LinkAssetLiabilityToClient(clientKey, assetLiabilityKey));
        };

        private It should_raise_a_investment_asset_added_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<InvestmentAssetAddedToClientEvent>
                (y => y.CompanyName == investmentAsset.CompanyName), legalEntityAssetLiabilityKey, 
                   (int)GenericKeyType.LegalEntityAssetLiability, Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_complete_unit_of_work = () =>
        {
            unitOfWork.WasToldTo(x => x.Complete());
        };

    }
}
