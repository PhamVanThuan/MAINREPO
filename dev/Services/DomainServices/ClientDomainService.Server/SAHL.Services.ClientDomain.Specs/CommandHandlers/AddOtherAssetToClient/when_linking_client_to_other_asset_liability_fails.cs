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

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddOtherAssetToClient
{
    public class when_linking_client_to_other_asset_liability_fails : WithCoreFakes
    {
        private static AddOtherAssetToClientCommandHandler handler;
        private static AddOtherAssetToClientCommand command;
        private static int clientKey;
        private static IAssetLiabilityDataManager assetLiabilityDataManager;
        private static OtherAssetModel otherAsset;
        private static int assetLiabilityKey, legalEntityAssetLiabilityKey;

        Establish context = () =>
        {
            assetLiabilityDataManager = An<IAssetLiabilityDataManager>();

            handler = new AddOtherAssetToClientCommandHandler(assetLiabilityDataManager, eventRaiser, unitOfWorkFactory);

            clientKey = 1111;
            otherAsset = new OtherAssetModel("Other asset", 1.1d, 0.1d);
            command = new AddOtherAssetToClientCommand(clientKey, otherAsset);

            assetLiabilityKey = 2222;
            assetLiabilityDataManager.WhenToldTo
                (x => x.SaveOtherAsset(Param.IsAny<OtherAssetModel>())).Return(assetLiabilityKey);

            legalEntityAssetLiabilityKey = 0;
            assetLiabilityDataManager.WhenToldTo
                (x => x.LinkAssetLiabilityToClient(Param.IsAny<int>(), Param.IsAny<int>())).Return(legalEntityAssetLiabilityKey);

            unitOfWorkFactory.WhenToldTo(x => x.Build()).Return(unitOfWork);
        };

        private Because of = () =>
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
                (x => x.SaveOtherAsset(Arg.Is<OtherAssetModel>(y => y.Description == otherAsset.Description)));
        };

        private It should_link_fixed_property_asset_to_client = () =>
        {
            assetLiabilityDataManager.WasToldTo
                (x => x.LinkAssetLiabilityToClient(clientKey, assetLiabilityKey));
        };

        private It should_contain_error_messages = () =>
        {
            messages.ErrorMessages().Count().ShouldEqual(1);
        };

        private It should_not_raise_other_asset_added_event = () =>
        {
            eventRaiser.WasNotToldTo
                (x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<OtherAssetAddedToClientEvent>
                    (y => y.Description == otherAsset.Description), legalEntityAssetLiabilityKey, 
                        (int)GenericKeyType.LegalEntityAssetLiability, Param.IsAny<IServiceRequestMetadata>()));
        };

    }
}