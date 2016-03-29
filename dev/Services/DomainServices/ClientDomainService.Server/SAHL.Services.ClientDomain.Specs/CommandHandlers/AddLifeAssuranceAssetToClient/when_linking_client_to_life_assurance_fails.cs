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
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddLifeAssuranceAssetToClient
{
    public class when_linking_client_to_life_assurance_fails : WithCoreFakes
    {
        private static AddLifeAssuranceAssetToClientCommandHandler handler;
        private static AddLifeAssuranceAssetToClientCommand command;
        private static int clientKey;
        private static LifeAssuranceAssetModel lifeAssuranceAsset;
        private static IAssetLiabilityDataManager assetLiabilityDataManager;
        private static int assetLiabilityKey, legalEntityAssetLiabilityKey;

        Establish context = () =>
        {
            assetLiabilityDataManager = An<IAssetLiabilityDataManager>();

            handler = new AddLifeAssuranceAssetToClientCommandHandler(assetLiabilityDataManager, eventRaiser, unitOfWorkFactory);

            clientKey = 1111;
            lifeAssuranceAsset = new LifeAssuranceAssetModel("Company name", 1d);
            command = new AddLifeAssuranceAssetToClientCommand(clientKey, lifeAssuranceAsset);

            assetLiabilityKey = 2222;
            assetLiabilityDataManager.WhenToldTo
                (x => x.SaveLifeAssuranceAsset(Param.IsAny<LifeAssuranceAssetModel>())).Return(assetLiabilityKey);

            legalEntityAssetLiabilityKey = 0;
            assetLiabilityDataManager.WhenToldTo
                (x => x.LinkAssetLiabilityToClient(Param.IsAny<int>(), Param.IsAny<int>())).Return(legalEntityAssetLiabilityKey);

            unitOfWorkFactory.WhenToldTo(x => x.Build()).Return(unitOfWork);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_begin_unit_of_work = () =>
        {
            unitOfWorkFactory.WasToldTo(x => x.Build());
        };

        private It should_add_life_assurance_asset = () =>
        {
            assetLiabilityDataManager.WasToldTo
                (x => x.SaveLifeAssuranceAsset(Arg.Is<LifeAssuranceAssetModel>
                    (y => y.CompanyName == lifeAssuranceAsset.CompanyName)));
        };

        private It should_link_fixed_property_asset_to_client = () =>
        {
            assetLiabilityDataManager.WasToldTo(x => x.LinkAssetLiabilityToClient(clientKey, assetLiabilityKey));
        };

        private It should_contain_error_messages = () =>
        {
            messages.ErrorMessages().Count().ShouldEqual(1);
        };

        private It should_not_raise_a_life_assurance_asset_added_event = () =>
        {
            eventRaiser.WasNotToldTo
                (x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<LifeAssuranceAssetAddedToClientEvent>
                    (y => y.CompanyName == lifeAssuranceAsset.CompanyName), legalEntityAssetLiabilityKey, 
                        (int)GenericKeyType.LegalEntityAssetLiability, Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}
