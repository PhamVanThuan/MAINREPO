using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers.AssetLiability;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using System;

namespace SAHL.Services.ClientDomain.CommandHandlers
{
    public class AddOtherAssetToClientCommandHandler : IDomainServiceCommandHandler<AddOtherAssetToClientCommand, OtherAssetAddedToClientEvent>
    {
        private IAssetLiabilityDataManager assetLiabilityManager;
        private IEventRaiser eventRaiser;
        private OtherAssetAddedToClientEvent @event;
        private IUnitOfWorkFactory unitOfWorkFactory;

        public AddOtherAssetToClientCommandHandler(IAssetLiabilityDataManager assetLiabilityManager, IEventRaiser eventRaiser, IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.assetLiabilityManager = assetLiabilityManager;
            this.eventRaiser = eventRaiser;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public ISystemMessageCollection HandleCommand(AddOtherAssetToClientCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            using (var uow = unitOfWorkFactory.Build())
            {
                int assetLiabilityKey = assetLiabilityManager.SaveOtherAsset(command.OtherAsset);
                if (assetLiabilityKey <= 0)
                {
                    messages.AddMessage(new SystemMessage("Failed to save the OtherAssetLiability.", SystemMessageSeverityEnum.Error));
                    return messages;
                }

                int clientOtherAssetLiabilityKey = assetLiabilityManager.LinkAssetLiabilityToClient(command.ClientKey, assetLiabilityKey);
                if (clientOtherAssetLiabilityKey <= 0)
                {
                    messages.AddMessage(new SystemMessage("Failed to link client to the OtherAssetLiability.", SystemMessageSeverityEnum.Error));
                    return messages;
                }

                @event = new OtherAssetAddedToClientEvent(DateTime.Now, clientOtherAssetLiabilityKey, command.OtherAsset.Description, command.OtherAsset.AssetValue, 
                    command.OtherAsset.LiabilityValue);
                eventRaiser.RaiseEvent(@event.Date, @event, clientOtherAssetLiabilityKey, (int)GenericKeyType.LegalEntityAssetLiability, metadata);

                uow.Complete();
            }

            return messages;
        }
    }
}