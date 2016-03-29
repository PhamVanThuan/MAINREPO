using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Events;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Events;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.AddressDomain.CommandHandlers
{
    public class LinkStreetAddressAsPostalAddressToClientCommandHandler : IDomainServiceCommandHandler<LinkStreetAddressAsPostalAddressToClientCommand, PostalStreetAddressLinkedToClientEvent>
    {
        private IServiceCommandRouter serviceCommandRouter;
        private IAddressDataManager addressDataManager;
        private ICombGuid combGuid;
        private ILinkedKeyManager linkedKeyManager;
        private IUnitOfWorkFactory uowFactory;
        private IEventRaiser eventRaiser;
        private PostalStreetAddressLinkedToClientEvent @event;

        public LinkStreetAddressAsPostalAddressToClientCommandHandler(IServiceCommandRouter serviceCommandRouter, IAddressDataManager addressDataManager,
            ICombGuid combGuid, ILinkedKeyManager linkedKeyManager, IUnitOfWorkFactory uowFactory, IEventRaiser eventRaiser)
        {
            this.serviceCommandRouter = serviceCommandRouter;
            this.addressDataManager = addressDataManager;
            this.combGuid = combGuid;
            this.linkedKeyManager = linkedKeyManager;
            this.uowFactory = uowFactory;
            this.eventRaiser = eventRaiser;
        }

        public ISystemMessageCollection HandleCommand(LinkStreetAddressAsPostalAddressToClientCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();

            using (var uow = uowFactory.Build())
            {
                var addressGuid = combGuid.Generate();

                var existingStreetAddresses = addressDataManager.FindAddressFromStreetAddress(command.StreetAddressModel);
                int streetAddressKey = existingStreetAddresses.Any() ? existingStreetAddresses.First().AddressKey : 0;

                if (!existingStreetAddresses.Any())
                {
                    var linkStreetAddressAsPostalCommand = new AddStreetAddressCommand(command.StreetAddressModel, addressGuid);
                    var linkMessageCollection = serviceCommandRouter.HandleCommand<AddStreetAddressCommand>(linkStreetAddressAsPostalCommand, metadata);
                    systemMessageCollection.AddMessages(linkMessageCollection.AllMessages);

                    if (!systemMessageCollection.HasErrors)
                    {
                        streetAddressKey = this.linkedKeyManager.RetrieveLinkedKey(addressGuid);
                        this.linkedKeyManager.DeleteLinkedKey(addressGuid);
                    }
                    else
                    {
                        uow.Complete();
                        return systemMessageCollection;
                    }
                }
                var clientAddress = new ClientAddressModel(command.ClientKey, streetAddressKey, AddressType.Postal);
                var addPostalAddressCommand = new AddClientAddressCommand(clientAddress, command.ClientAddressGuid);
                var addPostalAddressmessages = serviceCommandRouter.HandleCommand(addPostalAddressCommand, metadata);
                systemMessageCollection.AddMessages(addPostalAddressmessages.AllMessages);
                if (!systemMessageCollection.HasErrors)
                {
                    int clientAddressKey = linkedKeyManager.RetrieveLinkedKey(command.ClientAddressGuid);
                    @event = new PostalStreetAddressLinkedToClientEvent(DateTime.Now, command.StreetAddressModel.UnitNumber, command.StreetAddressModel.BuildingNumber, 
                        command.StreetAddressModel.BuildingName, command.StreetAddressModel.StreetNumber, command.StreetAddressModel.StreetName, command.StreetAddressModel.Suburb, 
                        command.StreetAddressModel.City, command.StreetAddressModel.Province, command.StreetAddressModel.PostalCode, command.ClientKey);

                    eventRaiser.RaiseEvent(DateTime.Now, @event, clientAddressKey, (int)GenericKeyType.LegalEntityAddress, metadata);
                }

                uow.Complete();
            }
            return systemMessageCollection;
        }
    }
}