using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Events;
using System;
using System.Linq;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Identity;

namespace SAHL.Services.AddressDomain.CommandHandlers
{
    public class LinkStreetAddressToPropertyCommandHandler :  IDomainServiceCommandHandler<LinkStreetAddressToPropertyCommand, StreetAddressLinkedToPropertyEvent>
    {
        private IServiceCommandRouter serviceCommandRouter;
        private IAddressDataManager addressDataManager;
        private ICombGuid combGuidGenerator;
        private ILinkedKeyManager linkedKeyManager;
        private IEvent @event;
        private IUnitOfWorkFactory uowFactory;
        private IEventRaiser eventRaiser;

        public LinkStreetAddressToPropertyCommandHandler(IServiceCommandRouter serviceCommandRouter, IAddressDataManager addressDataManager, ICombGuid combGuidGenerator,
            ILinkedKeyManager linkedKeyManager, IUnitOfWorkFactory uowFactory, IEventRaiser eventRaiser)
        {
            this.serviceCommandRouter = serviceCommandRouter;
            this.addressDataManager = addressDataManager;
            this.combGuidGenerator = combGuidGenerator;
            this.linkedKeyManager = linkedKeyManager;
            this.uowFactory = uowFactory;
            this.eventRaiser = eventRaiser;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(LinkStreetAddressToPropertyCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();

            using (var uow = uowFactory.Build())
            {
                var addresses = addressDataManager.FindAddressFromStreetAddress(command.StreetAddressModel);
                int? addressKey = null;
                if (addresses.Any())
                {
                    addressKey = addresses.First().AddressKey;
                }
                else
                {
                    var addressGuid = combGuidGenerator.Generate();
                    var addStreetAddressCommand = new AddStreetAddressCommand(command.StreetAddressModel, addressGuid);
                    systemMessageCollection = serviceCommandRouter.HandleCommand(addStreetAddressCommand, metadata);

                    addressKey = linkedKeyManager.RetrieveLinkedKey(addressGuid);
                    linkedKeyManager.DeleteLinkedKey(addressGuid);
                }

                if (!systemMessageCollection.HasErrors)
                {
                    addressDataManager.LinkAddressToProperty(command.PropertyKey, addressKey.Value);
                    @event = new StreetAddressLinkedToPropertyEvent(DateTime.Now, command.StreetAddressModel.UnitNumber, command.StreetAddressModel.BuildingNumber,
                        command.StreetAddressModel.BuildingName, command.StreetAddressModel.StreetNumber, command.StreetAddressModel.StreetName, command.StreetAddressModel.Suburb,
                        command.StreetAddressModel.City, command.StreetAddressModel.Province, command.StreetAddressModel.PostalCode, command.PropertyKey);

                    eventRaiser.RaiseEvent(DateTime.Now, @event, command.PropertyKey, (int)GenericKeyType.Property, metadata);
                }

                uow.Complete();
            }
            return systemMessageCollection;
        }

         
    }
}