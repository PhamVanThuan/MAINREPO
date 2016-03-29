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
    public class AddFixedLongTermInvestmentLiabilityToClientCommandHandler : IDomainServiceCommandHandler<AddFixedLongTermInvestmentLiabilityToClientCommand, 
        FixedLongTermInvestmentLiabilityAddedToClientEvent>
    {
        private IAssetLiabilityDataManager assetLiabilityDataManager;
        private IEventRaiser eventRaiser;
        private IUnitOfWorkFactory uowFactory;
        private FixedLongTermInvestmentLiabilityAddedToClientEvent @event;

        public AddFixedLongTermInvestmentLiabilityToClientCommandHandler(IAssetLiabilityDataManager assetLiabilityDataManager, IEventRaiser eventRaiser, IUnitOfWorkFactory uowFactory)
        {
            this.assetLiabilityDataManager = assetLiabilityDataManager;
            this.eventRaiser = eventRaiser;
            this.uowFactory = uowFactory;
        }

        public ISystemMessageCollection HandleCommand(AddFixedLongTermInvestmentLiabilityToClientCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();

            using (var uow = uowFactory.Build())
            {
                //add fixed liability
                var assetLiabilityKey = assetLiabilityDataManager.SaveFixedLongTermInvestmentLiability(command.FixedLongTermInvestmentLiabilityModel);
                if (assetLiabilityKey < 1)
                {
                    messages.AddMessage(new SystemMessage("An error occured when saving the fixed long term investment.", SystemMessageSeverityEnum.Error));
                    uow.Complete();
                    return messages;
                }

                //link fixed liability to client
                var assetLiabilityClientkey = assetLiabilityDataManager.LinkAssetLiabilityToClient(command.ClientKey, assetLiabilityKey);
                if (assetLiabilityClientkey < 1)
                {
                    messages.AddMessage(new SystemMessage("An error occurred when linking the fixed long term investment to the client.", SystemMessageSeverityEnum.Error));
                    uow.Complete();
                    return messages;
                }

                //raise event
                @event = new FixedLongTermInvestmentLiabilityAddedToClientEvent(DateTime.Now, command.FixedLongTermInvestmentLiabilityModel.CompanyName, 
                    command.FixedLongTermInvestmentLiabilityModel.LiabilityValue);
                eventRaiser.RaiseEvent(@event.Date, @event, assetLiabilityClientkey, (int)GenericKeyType.LegalEntityAssetLiability, metadata);

                uow.Complete();
            }

            return messages;
        }
    }
}