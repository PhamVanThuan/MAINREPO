using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
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
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.AddressDomain.CommandHandlers
{
    public class LinkPostalAddressToClientCommandHandler : IDomainServiceCommandHandler<LinkPostalAddressToClientCommand, PostalAddressLinkedToClientEvent>
    {
        private IServiceCommandRouter serviceCommandRouter;
        private IAddressDataManager addressDataManager;
        private ICombGuid combGuid;
        private ILinkedKeyManager linkedKeyManager;
        private PostalAddressLinkedToClientEvent @event;
        private IUnitOfWorkFactory uowFactory;
        private IEventRaiser eventRaiser;

        public LinkPostalAddressToClientCommandHandler(IServiceCommandRouter serviceCommandRouter, IAddressDataManager addressDataManager, ICombGuid combGuid,
            ILinkedKeyManager linkedKeyManager, IUnitOfWorkFactory uowFactory, IEventRaiser eventRaiser)
        {
            this.serviceCommandRouter = serviceCommandRouter;
            this.addressDataManager = addressDataManager;
            this.combGuid = combGuid;
            this.linkedKeyManager = linkedKeyManager;
            this.uowFactory = uowFactory;
            this.eventRaiser = eventRaiser;
        }

        public ISystemMessageCollection HandleCommand(LinkPostalAddressToClientCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();

            using (var uow = uowFactory.Build())
            {
                IEnumerable<AddressDataModel> postalAddresses;

                postalAddresses = addressDataManager.FindPostalAddressFromAddressValues(command.PostalAddressModel);
                int postalAddressKey = 0;
                if (!postalAddresses.Any())
                {
                    Guid addressGuid = this.combGuid.Generate();
                    AddPostalAddressCommand addPostalAddressCommand = new AddPostalAddressCommand(command.PostalAddressModel, addressGuid);
                    var addStreetAddressMessages = serviceCommandRouter.HandleCommand<AddPostalAddressCommand>(addPostalAddressCommand, metadata);
                    systemMessageCollection.AddMessages(addStreetAddressMessages.AllMessages);

                    if (!systemMessageCollection.HasErrors)
                    {
                        postalAddressKey = this.linkedKeyManager.RetrieveLinkedKey(addressGuid);
                        this.linkedKeyManager.DeleteLinkedKey(addressGuid);
                    }
                }
                else
                {
                    postalAddressKey = postalAddresses.First().AddressKey;
                }

                if (!systemMessageCollection.HasErrors)
                {
                    var clientAddress = new ClientAddressModel(command.ClientKey, postalAddressKey, AddressType.Postal);
                    var addClientAddressCommand = new AddClientAddressCommand(clientAddress, command.ClientAddressGuid);
                    var addClientAddressMessages = serviceCommandRouter.HandleCommand<AddClientAddressCommand>(addClientAddressCommand, metadata);

                    systemMessageCollection.AddMessages(addClientAddressMessages.AllMessages);
                }

                //define event here
                if (!systemMessageCollection.HasErrors)
                {
                    int clientAddressKey = linkedKeyManager.RetrieveLinkedKey(command.ClientAddressGuid);
                    @event = new PostalAddressLinkedToClientEvent(DateTime.Now, command.PostalAddressModel.BoxNumber, command.PostalAddressModel.PostNetSuiteNumber,
                        command.PostalAddressModel.PostOffice, command.PostalAddressModel.City, command.PostalAddressModel.Province, command.PostalAddressModel.PostalCode, 
                        command.PostalAddressModel.AddressFormat,
                        command.ClientKey, clientAddressKey);

                    eventRaiser.RaiseEvent(DateTime.Now, @event, clientAddressKey, (int)GenericKeyType.LegalEntityAddress, metadata);
                }

                uow.Complete();
            }
            return systemMessageCollection;
        }
    }
}