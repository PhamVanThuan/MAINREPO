using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
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
    public class when_linking_a_street_address_that_is_not_in_our_system : WithCoreFakes
    {
        private static LinkStreetAddressToPropertyCommand command;
        private static LinkStreetAddressToPropertyCommandHandler handler;
        private static IAddressDataManager addressDataManager;
        private static StreetAddressModel streetAddress;
        private static int propertyKey;
        private static Guid addressGuid;
        private static int addressKey;
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

            //address does not exist
            addresses = Enumerable.Empty<AddressDataModel>();
            addressDataManager.WhenToldTo(x => x.FindAddressFromStreetAddress(Param.IsAny<StreetAddressModel>())).Return(addresses);

            //create guid
            addressGuid = CombGuid.Instance.Generate();
            combGuid.WhenToldTo(c => c.Generate()).Return(addressGuid);

            //add address
            messages = SystemMessageCollection.Empty();
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param.IsAny<AddStreetAddressCommand>(), serviceRequestMetaData)).Return(messages);

            //retrieve linked key
            addressKey = 13;
            linkedKeyManager.WhenToldTo(x => x.RetrieveLinkedKey(Param.IsAny<Guid>())).Return(addressKey);
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_if_street_address_already_exists_in_repository = () =>
        {
            addressDataManager.WasToldTo(x => x.FindAddressFromStreetAddress(Param.IsAny<StreetAddressModel>()));
        };

        private It should_add_a_new_street_address = () =>
        {
            serviceCommandRouter.WasToldTo(x =>
                x.HandleCommand(Arg.Is<AddStreetAddressCommand>(y => y.StreetAddressModel.StreetName == streetAddress.StreetName),
                    serviceRequestMetaData));
        };

        private It should_get_the_key_of_the_new_address = () =>
        {
            linkedKeyManager.WasToldTo(x => x.RetrieveLinkedKey(addressGuid));
        };

        private It should_remove_the_linked_key_for_the_address = () =>
        {
            linkedKeyManager.WasToldTo(x => x.DeleteLinkedKey(addressGuid));
        };

        private It should_link_street_address_to_property = () =>
        {
            addressDataManager.WasToldTo(a => a.LinkAddressToProperty(command.PropertyKey, addressKey));
        };

        private It should_raise_a_street_address_linked_to_property_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                Arg.Is<StreetAddressLinkedToPropertyEvent>
                    (y => y.PropertyKey == command.PropertyKey && y.BuildingNumber == command.StreetAddressModel.BuildingNumber
                        && y.StreetName == streetAddress.StreetName),
                propertyKey,
                (int)GenericKeyType.LegalEntityAddress,
                Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}
