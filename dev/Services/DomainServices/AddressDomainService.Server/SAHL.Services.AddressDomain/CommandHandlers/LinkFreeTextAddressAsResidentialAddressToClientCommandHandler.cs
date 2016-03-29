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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.AddressDomain.CommandHandlers
{
    public class LinkFreeTextAddressAsResidentialAddressToClientCommandHandler : IDomainServiceCommandHandler<LinkFreeTextAddressAsResidentialAddressToClientCommand, 
        FreeTextResidentialAddressLinkedToClientEvent>
    {
        private IAddressDataManager addressDataManager;
        private ICombGuid combGuid;
        private IEventRaiser eventRaiser;
        private ILinkedKeyManager linkedKeyManager;
        private IServiceCommandRouter serviceCommandRouter;
        private IUnitOfWorkFactory unitOfWorkFactory;

        public LinkFreeTextAddressAsResidentialAddressToClientCommandHandler(IServiceCommandRouter serviceCommandRouter, IAddressDataManager addressDataManager, ICombGuid combGuid, 
            ILinkedKeyManager linkedKeyManager, IUnitOfWorkFactory unitOfWorkFactory, IEventRaiser eventRaiser)
        {
            this.serviceCommandRouter = serviceCommandRouter;
            this.addressDataManager = addressDataManager;
            this.combGuid = combGuid;
            this.linkedKeyManager = linkedKeyManager;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.eventRaiser = eventRaiser;
        }

        public ISystemMessageCollection HandleCommand(LinkFreeTextAddressAsResidentialAddressToClientCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();

            using (var uow = unitOfWorkFactory.Build())
            {
                var existingFreeTextAddresses = addressDataManager.FindAddressFromFreeTextAddress(command.FreeTextAddressModel);
                int freeTextAddressKey = existingFreeTextAddresses.Any() ? existingFreeTextAddresses.First().AddressKey : 0;
                if (!existingFreeTextAddresses.Any())
                {
                    Guid addressGuid = this.combGuid.Generate();
                    AddFreeTextAddressCommand addAddressCommand = new AddFreeTextAddressCommand(command.FreeTextAddressModel, addressGuid);
                    messages.Aggregate(this.serviceCommandRouter.HandleCommand(addAddressCommand, metadata));
                    if (!messages.HasErrors)
                    {
                        freeTextAddressKey = this.linkedKeyManager.RetrieveLinkedKey(addressGuid);
                        this.linkedKeyManager.DeleteLinkedKey(addressGuid);
                    }
                    else
                    {
                        uow.Complete();
                        return messages;
                    }
                }

                var clientAddress = new ClientAddressModel(command.ClientKey, freeTextAddressKey, AddressType.Residential);
                var addClientAddressCommand = new AddClientAddressCommand(clientAddress, command.ClientAddressGuid);
                messages.Aggregate(serviceCommandRouter.HandleCommand(addClientAddressCommand, metadata));
                if (!messages.HasErrors)
                {
                    int clientAddressKey = linkedKeyManager.RetrieveLinkedKey(command.ClientAddressGuid);
                    var @event = new FreeTextResidentialAddressLinkedToClientEvent(DateTime.Now, command.FreeTextAddressModel.FreeText1, command.FreeTextAddressModel.FreeText2, 
                        command.FreeTextAddressModel.FreeText3, command.FreeTextAddressModel.FreeText4, command.FreeTextAddressModel.FreeText5, command.FreeTextAddressModel.Country, 
                        command.FreeTextAddressModel.AddressFormat, command.ClientKey, clientAddressKey);

                    eventRaiser.RaiseEvent(DateTime.Now, @event, clientAddressKey, (int)GenericKeyType.LegalEntityAddress, metadata);
                }
                uow.Complete();
            }
            return messages;
        }
    }
}