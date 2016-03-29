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
    public class AddLifeAssuranceAssetToClientCommandHandler : IDomainServiceCommandHandler<AddLifeAssuranceAssetToClientCommand, LifeAssuranceAssetAddedToClientEvent>
    {
        private IAssetLiabilityDataManager assetLiabilityDataManager;
        private IEventRaiser eventRaiser;
        private LifeAssuranceAssetAddedToClientEvent @event;
        private IUnitOfWorkFactory unitOfWorkFactory;

        public AddLifeAssuranceAssetToClientCommandHandler(IAssetLiabilityDataManager assetLiabilityDataManager, IEventRaiser eventRaiser, IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.assetLiabilityDataManager = assetLiabilityDataManager;
            this.eventRaiser = eventRaiser;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(AddLifeAssuranceAssetToClientCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            using (var uow = unitOfWorkFactory.Build())
            {
                var assetLiabilityKey = assetLiabilityDataManager.SaveLifeAssuranceAsset(command.LifeAssuranceAsset);
                if (assetLiabilityKey <= 0)
                {
                    messages.AddMessage(new SystemMessage("Failed to save LifeAssuranceAssetLiability.", SystemMessageSeverityEnum.Error));
                    return messages;
                }

                var clientAssetLiabilityKey = assetLiabilityDataManager.LinkAssetLiabilityToClient(command.ClientKey, assetLiabilityKey);
                if (clientAssetLiabilityKey <= 0)
                {
                    messages.AddMessage(new SystemMessage("Failed to link client to the LifeAssuranceAssetLiability.", SystemMessageSeverityEnum.Error));
                    return messages;
                }

                @event = new LifeAssuranceAssetAddedToClientEvent(DateTime.Now, command.LifeAssuranceAsset.CompanyName, command.LifeAssuranceAsset.SurrenderValue);
                eventRaiser.RaiseEvent(@event.Date, @event, clientAssetLiabilityKey, (int)GenericKeyType.LegalEntityAssetLiability, metadata);

                uow.Complete();
            }

            return messages;
        }
    }
}