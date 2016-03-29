using System;
using System.Collections.Generic;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;

namespace SAHL.Services.ClientDomain.CommandHandlers
{
    public class AddMarketingOptionsToClientCommandHandler : IDomainServiceCommandHandler<AddMarketingOptionsToClientCommand,
        MarketingOptionsAddedEvent>
    {
        private IClientDataManager clientDataManager;
        private MarketingOptionsAddedEvent @event;
        private IUnitOfWorkFactory uowFactory;
        private IEventRaiser eventRaiser;

        public AddMarketingOptionsToClientCommandHandler(IClientDataManager clientDataManager, IEventRaiser eventRaiser, IUnitOfWorkFactory uowFactory)
        {            
            this.clientDataManager = clientDataManager;
            this.eventRaiser = eventRaiser;
            this.uowFactory = uowFactory;           
        }

        public ISystemMessageCollection HandleCommand(AddMarketingOptionsToClientCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();

            using (var uow = uowFactory.Build())
            {
                foreach (var marketingOption in command.ClientMarketingOptionsCollection)
                {
                    var clientMarketingOptionExists = clientDataManager.DoesClientMarketingOptionExist(command.ClientKey, (int)marketingOption.MarketingOptionKey);
                    if (!clientMarketingOptionExists)
                    {
                        clientDataManager.AddNewMarketingOptions(new LegalEntityMarketingOptionDataModel(command.ClientKey, (int)marketingOption.MarketingOptionKey, DateTime.Now, "x2"));
                    }
                }
                @event = new MarketingOptionsAddedEvent(DateTime.Now, command.ClientMarketingOptionsCollection);
                eventRaiser.RaiseEvent(DateTime.Now, @event, command.ClientKey, (int)GenericKeyType.LegalEntity, metadata);
               
                uow.Complete();
            }

            return messages;
        }
    }
}