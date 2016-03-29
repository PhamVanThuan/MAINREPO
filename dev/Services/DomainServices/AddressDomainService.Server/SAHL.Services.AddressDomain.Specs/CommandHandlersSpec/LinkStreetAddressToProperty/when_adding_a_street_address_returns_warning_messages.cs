using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.AddressDomain.CommandHandlers;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Events;
using SAHL.Services.Interfaces.AddressDomain.Model;

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.LinkStreetAddressToProperty
{
    public class when_adding_a_street_address_returns_warning_messages : WithCoreFakes
    {
        private static LinkStreetAddressToPropertyCommandHandler handler;
        private static LinkStreetAddressToPropertyCommand command;
        private static IAddressDataManager addressDataManager;
        private static ISystemMessageCollection systemMessageCollection;
        private static int propertyKey, addressKey;
        private static StreetAddressModel streetAddressModel;
        private static Guid addressGuid;

        private Establish context = () =>
        {
            //set up mock objects
            addressDataManager = An<IAddressDataManager>();

            //new up handler
            handler = new LinkStreetAddressToPropertyCommandHandler(serviceCommandRouter,
                addressDataManager,
                combGuid,
                linkedKeyManager,
                unitOfWorkFactory,
                eventRaiser);

            //new up command
            propertyKey = 4;
            streetAddressModel = new StreetAddressModel("", "", "", "7", "Maryland Avenue", "Durban North", "Durban", "Kwazulu-Natal", "4051");
            command = new LinkStreetAddressToPropertyCommand(streetAddressModel, propertyKey);

            //property exists

            //address does not exist
            addressDataManager.WhenToldTo(x => x.FindAddressFromStreetAddress(Param.IsAny<StreetAddressModel>()))
                .Return(new List<AddressDataModel>());

            //create guid
            addressGuid = CombGuid.Instance.Generate();
            combGuid.WhenToldTo(x => x.Generate()).Return(addressGuid);

            //add address returns warnings
            systemMessageCollection = SystemMessageCollection.Empty();
            systemMessageCollection.AddMessage(new SystemMessage("Warning encountered.", SystemMessageSeverityEnum.Warning));
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param.IsAny<AddStreetAddressCommand>(), serviceRequestMetaData))
                .Return(systemMessageCollection);

            //retrieve linked key
            addressKey = 12;

            linkedKeyManager.WhenToldTo(l => l.RetrieveLinkedKey(Param.IsAny<Guid>())).Return(addressKey);

            //delete linked key

            //add address to property
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_if_street_address_is_already_on_system = () =>
        {
            addressDataManager.WasToldTo(x => x.FindAddressFromStreetAddress(streetAddressModel));
        };

        private It should_add_the_street_address = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param.IsAny<AddStreetAddressCommand>(), serviceRequestMetaData));
        };

        private It should_get_the_address_key_of_newly_created_entry = () =>
        {
            linkedKeyManager.WasToldTo(x => x.RetrieveLinkedKey(addressGuid));
        };

        private It should_remove_the_address_guid = () =>
        {
            linkedKeyManager.WasToldTo(x => x.DeleteLinkedKey(addressGuid));
        };

        private It should_return_the_error_messages_from_the_sub_command = () =>
        {
            messages.WarningMessages().First().Message.ShouldEqual("Warning encountered.");
        };

        private It should_still_link_street_address_to_property = () =>
        {
            addressDataManager.WasToldTo(a => a.LinkAddressToProperty(command.PropertyKey, addressKey));
        };

        private It should_not_raise_a_street_address_linked_to_property_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                Param.IsAny<StreetAddressLinkedToPropertyEvent>(),
                propertyKey,
                (int)GenericKeyType.LegalEntityAddress,
                Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}
