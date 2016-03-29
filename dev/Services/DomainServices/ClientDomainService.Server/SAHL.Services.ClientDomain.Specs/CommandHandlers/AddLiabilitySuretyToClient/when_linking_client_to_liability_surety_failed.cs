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

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddLiabilitySuretyToClient
{
    public class when_linking_client_to_liability_surety_failed : WithCoreFakes
    {
        private static AddLiabilitySuretyToClientCommand command;
        private static AddLiabilitySuretyToClientCommandHandler handler;
        private static LiabilitySuretyModel model;
        private static IAssetLiabilityDataManager assetLiabilityDataManager;
        private static int legalEntityAssetLiabilityKey, assetLiabilityKey, clientKey;

        private Establish context = () =>
        {
            assetLiabilityDataManager = An<IAssetLiabilityDataManager>();
            var assetValue = 1;
            var liabilityValue = 2;
            var description = "Test";
            clientKey = 1234;
            model = new LiabilitySuretyModel(assetValue, liabilityValue, description);
            command = new AddLiabilitySuretyToClientCommand(model, clientKey);
            handler = new AddLiabilitySuretyToClientCommandHandler(assetLiabilityDataManager, eventRaiser, unitOfWorkFactory);

            unitOfWorkFactory.WhenToldTo(x => x.Build()).Return(unitOfWork);

            assetLiabilityKey = 12345;
            assetLiabilityDataManager.WhenToldTo
                (x => x.SaveLiabilitySurety(Param.IsAny<LiabilitySuretyModel>())).Return(assetLiabilityKey);

            legalEntityAssetLiabilityKey = 0;
            assetLiabilityDataManager.WhenToldTo
                (x => x.LinkAssetLiabilityToClient(Param.IsAny<int>(), Param.IsAny<int>())).Return(legalEntityAssetLiabilityKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_add_the_liability_surety = () =>
        {
            assetLiabilityDataManager.WasToldTo
                (x => x.SaveLiabilitySurety(Arg.Is<LiabilitySuretyModel>
                    (y => y.Description == model.Description && y.AssetValue == model.AssetValue && y.LiabilityValue == model.LiabilityValue)));
        };

        private It should_link_the_fixed_long_term_investment_liability_to_client = () =>
        {
            assetLiabilityDataManager.WasToldTo(x => x.LinkAssetLiabilityToClient(clientKey, assetLiabilityKey));
        };

        private It should_contain_error_messages = () =>
        {
            messages.ErrorMessages().Count().ShouldEqual(1);
        };

        private It should_not_raise_a_long_term_investment_liability_added_event = () =>
        {
            eventRaiser.WasNotToldTo
                (x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<LiabilitySuretyAddedToClientEvent>
                    (y => y.Description == model.Description), legalEntityAssetLiabilityKey, 
                      (int)GenericKeyType.LegalEntityAssetLiability, Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}