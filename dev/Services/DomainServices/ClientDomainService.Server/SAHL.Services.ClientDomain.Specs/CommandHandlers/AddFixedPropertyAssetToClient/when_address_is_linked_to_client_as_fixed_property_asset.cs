using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers.AssetLiability;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;


namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddFixedPropertyAssetToClient
{
    class when_address_is_linked_to_client_as_fixed_property_asset : WithCoreFakes
    {
        private static AddFixedPropertyAssetToClientCommandHandler handler;
        private static AddFixedPropertyAssetToClientCommand command;
        private static FixedPropertyAssetModel fixedPropertyAsset;
        private static IDomainRuleManager<FixedPropertyAssetModel> domainRuleManager;
        private static IDomainQueryServiceClient domainQueryService;
        private static IAssetLiabilityDataManager assetLiabilityDataManager;
        private static int clientKey;
        private static ISystemMessageCollection expectedMessages;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<FixedPropertyAssetModel>>();
            assetLiabilityDataManager = An<IAssetLiabilityDataManager>();
            domainQueryService = An<IDomainQueryServiceClient>();

            handler = new AddFixedPropertyAssetToClientCommandHandler
                (domainRuleManager, domainQueryService, assetLiabilityDataManager, unitOfWorkFactory, eventRaiser);

            clientKey = 1111;
            fixedPropertyAsset = new FixedPropertyAssetModel(DateTime.Now, 2222, 1.2d, 0.1d);
            command = new AddFixedPropertyAssetToClientCommand(clientKey, fixedPropertyAsset);

            expectedMessages = SystemMessageCollection.Empty();
            domainRuleManager.WhenToldTo
              (x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<FixedPropertyAssetModel>()))
                .Callback<ISystemMessageCollection, FixedPropertyAssetModel>((y, z) => { y.AddMessages(expectedMessages.AllMessages); });

            assetLiabilityDataManager.WhenToldTo
                (x => x.CheckIsAddressLinkedToClientAsFixedPropertyAsset(Param.IsAny<int>(), Param.IsAny<int>())).Return(true);

            var isAddressAClientAddressQuery = new ServiceQueryResult<IsAddressAClientAddressQueryResult>(
                new IsAddressAClientAddressQueryResult[]
                {
                    new IsAddressAClientAddressQueryResult() { AddressIsAClientAddress = true }
                });

            domainQueryService.WhenToldTo(d => d.PerformQuery(Param.IsAny<IsAddressAClientAddressQuery>()))
                .Return<IsAddressAClientAddressQuery>(y => { y.Result = isAddressAClientAddressQuery; return messages; });
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_address_is_linked_to_client = () =>
        {
            domainQueryService.WasToldTo(x => x.PerformQuery(Param.IsAny<IsAddressAClientAddressQuery>()));
        };

        private It should_check_fixed_property_is_not_already_linked_as_asset_for_client = () =>
        {
            assetLiabilityDataManager.WasToldTo(x => x.CheckIsAddressLinkedToClientAsFixedPropertyAsset(clientKey, fixedPropertyAsset.AddressKey));
        };

        It should_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeTrue();
        };

        private It should_not_begin_unit_of_work = () =>
        {
            unitOfWorkFactory.WasNotToldTo(x => x.Build());
        };

        private It should_not_add_fixed_property_asset = () =>
        {
            assetLiabilityDataManager.WasNotToldTo(x => x.SaveFixedPropertyAsset(Param.IsAny<FixedPropertyAssetModel>()));
        };

        private It should_not_link_fixed_property_asset_to_client = () =>
        {
            assetLiabilityDataManager.WasNotToldTo(x => x.LinkAssetLiabilityToClient(Param.IsAny<int>(), Param.IsAny<int>()));
        };

        private It should_not_raise_a_fixed_property_asset_added_event = () =>
        {
            eventRaiser.WasNotToldTo
                (x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<FixedPropertyAssetAddedToClientEvent>()
                    , Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_not_complete_unit_of_work = () =>
        {
            unitOfWork.WasNotToldTo(x => x.Complete());
        };
    }
}
