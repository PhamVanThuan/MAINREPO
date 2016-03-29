using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers.AssetLiability;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.CommandHandlers
{
    public class AddFixedPropertyAssetToClientCommandHandler : IDomainServiceCommandHandler<AddFixedPropertyAssetToClientCommand, FixedPropertyAssetAddedToClientEvent>
    {
        private IDomainRuleManager<FixedPropertyAssetModel> domainRuleManager;
        private IDomainQueryServiceClient domainQueryService;
        private IAssetLiabilityDataManager assetLiabilityDataManager;
        private IUnitOfWorkFactory unitOfWorkFactory;
        private IEventRaiser eventRaiser;

        public AddFixedPropertyAssetToClientCommandHandler(
              IDomainRuleManager<FixedPropertyAssetModel> domainRuleManager
            , IDomainQueryServiceClient domainQueryService
            , IAssetLiabilityDataManager assetLiabilityDataManager
            , IUnitOfWorkFactory unitOfWorkFactory
            , IEventRaiser eventRaiser
        )
        {
            this.assetLiabilityDataManager = assetLiabilityDataManager;
            this.domainRuleManager = domainRuleManager;
            this.domainQueryService = domainQueryService;
            this.eventRaiser = eventRaiser;
            this.unitOfWorkFactory = unitOfWorkFactory;

            this.domainRuleManager.RegisterRule(new FixedPropertyAssetAcquiredDateCannotBeInTheFutureRule<FixedPropertyAssetModel>());
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(AddFixedPropertyAssetToClientCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();

            var isAddressAClientAddressQuery = new IsAddressAClientAddressQuery(command.FixedPropertyAsset.AddressKey, command.ClientKey);
            domainQueryService.PerformQuery(isAddressAClientAddressQuery);
            bool addressIsAClientAddress = isAddressAClientAddressQuery.Result.Results.First().AddressIsAClientAddress;
            if (!addressIsAClientAddress)
            {
                messages.AddMessage(new SystemMessage("Address is not linked to client", SystemMessageSeverityEnum.Error));
            }

            bool addressAlreadyLinkedToClientAsFixedPropertyAsset = assetLiabilityDataManager.CheckIsAddressLinkedToClientAsFixedPropertyAsset(command.ClientKey, 
                command.FixedPropertyAsset.AddressKey);
            if (addressAlreadyLinkedToClientAsFixedPropertyAsset)
            {
                messages.AddMessage(new SystemMessage("Fixed property asset is already linked to client", SystemMessageSeverityEnum.Error));
            }

            if (addressIsAClientAddress && !addressAlreadyLinkedToClientAsFixedPropertyAsset)
            {
                domainRuleManager.ExecuteRules(messages, command.FixedPropertyAsset);

                if (!messages.HasErrors)
                {
                    using (var uow = unitOfWorkFactory.Build())
                    {
                        int assetLiabilityKey = assetLiabilityDataManager.SaveFixedPropertyAsset(command.FixedPropertyAsset);
                        int clientAssetLiabilityKey = assetLiabilityDataManager.LinkAssetLiabilityToClient(command.ClientKey, assetLiabilityKey);

                        eventRaiser.RaiseEvent(
                              DateTime.Now
                            , new FixedPropertyAssetAddedToClientEvent(DateTime.Now, command.FixedPropertyAsset.DateAquired, command.FixedPropertyAsset.AddressKey, 
                                command.FixedPropertyAsset.AssetValue, command.FixedPropertyAsset.LiabilityValue)
                            , clientAssetLiabilityKey
                            , (int)GenericKeyType.LegalEntityAssetLiability
                            , metadata
                        );

                        uow.Complete();
                    }
                }
            }

            return messages;
        }
    }
}
