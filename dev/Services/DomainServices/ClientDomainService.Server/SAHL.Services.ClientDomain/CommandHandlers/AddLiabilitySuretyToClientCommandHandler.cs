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
    public class AddLiabilitySuretyToClientCommandHandler : IDomainServiceCommandHandler<AddLiabilitySuretyToClientCommand, LiabilitySuretyAddedToClientEvent>
    {
        private IAssetLiabilityDataManager assetLiabilityDataManager;
        private IEventRaiser eventRaiser;
        private LiabilitySuretyAddedToClientEvent @event;
        private IUnitOfWorkFactory uowFactory;

        public AddLiabilitySuretyToClientCommandHandler(IAssetLiabilityDataManager assetLiabilityDataManager, IEventRaiser eventRaiser, IUnitOfWorkFactory uowFactory)
        {
            this.assetLiabilityDataManager = assetLiabilityDataManager;
            this.eventRaiser = eventRaiser;
            this.uowFactory = uowFactory;
        }

        public ISystemMessageCollection HandleCommand(AddLiabilitySuretyToClientCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            using (var uow = uowFactory.Build())
            {
                var suretyLiabilityKey = assetLiabilityDataManager.SaveLiabilitySurety(command.LiabilitySuretyModel);
                if (suretyLiabilityKey <= 0)
                {
                    messages.AddMessage(new SystemMessage("Failed to save the Liability Surety.", SystemMessageSeverityEnum.Error));
                    return messages;
                }

                var clientSuretyLiabilityKey = assetLiabilityDataManager.LinkAssetLiabilityToClient(command.ClientKey, suretyLiabilityKey);
                if (clientSuretyLiabilityKey <= 0)
                {
                    messages.AddMessage(new SystemMessage("Failed to link the Client to the Liability Surety.", SystemMessageSeverityEnum.Error));
                    return messages;
                }

                @event = new LiabilitySuretyAddedToClientEvent(DateTime.Now, suretyLiabilityKey, clientSuretyLiabilityKey, command.LiabilitySuretyModel.AssetValue, 
                    command.LiabilitySuretyModel.LiabilityValue, command.LiabilitySuretyModel.Description);
                eventRaiser.RaiseEvent(@event.Date, @event, clientSuretyLiabilityKey, (int)GenericKeyType.LegalEntityAssetLiability, metadata);

                uow.Complete();
            }


            return messages;
        }
    }
}