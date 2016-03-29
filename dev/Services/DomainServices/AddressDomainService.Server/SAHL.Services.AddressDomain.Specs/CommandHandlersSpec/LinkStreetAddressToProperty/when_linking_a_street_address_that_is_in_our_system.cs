using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.AddressDomain.CommandHandlers;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Events;
using SAHL.Services.Interfaces.AddressDomain.Model;

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.LinkStreetAddressToProperty
{
    public class when_linking_a_street_address_that_is_in_our_system : WithCoreFakes
    {
        private static LinkStreetAddressToPropertyCommand command;
        private static LinkStreetAddressToPropertyCommandHandler handler;
        private static IAddressDataManager addressDataManager;
        private static StreetAddressModel streetAddress;
        private static int propertyKey;
        private static IEnumerable<AddressDataModel> addresses;

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
            streetAddress = new StreetAddressModel("", "", "", "12", "Clement Stott Rd", "Bothas Hill", "Bothas Hill", "Kwazulu Natal", "3660");
            propertyKey = 77;
            command = new LinkStreetAddressToPropertyCommand(streetAddress, propertyKey);

            //property exists

            //address exist
            addresses = new[]
            {
                new AddressDataModel(1234,
                    1,
                    null,
                    null,
                    null,
                    null,
                    "12",
                    "Blue Road",
                    1234,
                    null,
                    "South Africa",
                    "Gauteng",
                    "Gauteng",
                    "Sandton",
                    "1234",
                    "test",
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null)
            };
            addressDataManager.WhenToldTo(x => x.FindAddressFromStreetAddress(Param.IsAny<StreetAddressModel>())).Return(addresses);

            //add address to property
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_if_street_address_already_exists_in_repository = () =>
        {
            addressDataManager.WasToldTo(x => x.FindAddressFromStreetAddress(Param.IsAny<StreetAddressModel>()));
        };

        private It should_not_add_new_street_address = () =>
        {
            serviceCommandRouter.WasNotToldTo(x => x.HandleCommand(Param.IsAny<AddStreetAddressCommand>(), serviceRequestMetaData));
        };

        private It should_not_use_the_linked_key_manager_to_retrieve_the_addressKey = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.RetrieveLinkedKey(Param.IsAny<Guid>()));
        };

        private It should_not_use_the_linked_key_manager_to_remove_the_addressKey = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.RetrieveLinkedKey(Param.IsAny<Guid>()));
        };

        private It should_link_the_address_to_the_property = () =>
        {
            addressDataManager.WasToldTo(x => x.LinkAddressToProperty(propertyKey, addresses.First().AddressKey));
        };

        private It should_return_an_empty_system_messages_collection = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };

        private It should_raise_a_street_address_linked_to_property_event = () =>
        {
            eventRaiser.WasToldTo(
                x => x.RaiseEvent(Param.IsAny<DateTime>(),
                    Arg.Is<StreetAddressLinkedToPropertyEvent>(y => y.PropertyKey == propertyKey
                        && y.BuildingName == streetAddress.BuildingName
                        && y.PostalCode == streetAddress.PostalCode),
                    command.PropertyKey,
                    (int)GenericKeyType.Property,
                    Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}
