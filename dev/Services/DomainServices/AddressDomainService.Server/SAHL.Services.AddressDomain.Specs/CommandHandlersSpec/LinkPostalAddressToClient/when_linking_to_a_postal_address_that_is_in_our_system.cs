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

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.LinkPostalAddressToClient
{
    public class when_linking_to_a_postal_address_that_is_in_our_system : WithCoreFakes
    {
        private static LinkPostalAddressToClientCommandHandler handler;
        private static LinkPostalAddressToClientCommand command;
        private static IAddressDataManager addressDataManager;
        private static int clientKey;
        private static PostalAddressModel postalAddress;
        private static Guid clientAddressGuid;
        private static Guid guid;
        private static IEnumerable<AddressDataModel> existingAddresses;
        private static ISystemMessageCollection serviceMessageCollection;
        private static int linkedClientAddressKey;

        private Establish context = () =>
        {
            //set up mock objects
            addressDataManager = An<IAddressDataManager>();
            guid = CombGuid.Instance.Generate();
            combGuid.WhenToldTo(x => x.Generate()).Return(guid);

            //new up handler
            handler = new LinkPostalAddressToClientCommandHandler(serviceCommandRouter,
                addressDataManager,
                combGuid,
                linkedKeyManager,
                unitOfWorkFactory,
                eventRaiser);

            //new up command
            postalAddress = new PostalAddressModel("1883", "", "Hillcrest", "Kwazulu Natal", "Hillcrest", "3650", AddressFormat.Box);
            clientKey = 12;
            clientAddressGuid = CombGuid.Instance.Generate();
            command = new LinkPostalAddressToClientCommand(postalAddress, clientKey, clientAddressGuid);

            //postal address exists
            existingAddresses = new AddressDataModel[]
            {
                new AddressDataModel(1234,
                    1,
                    null,
                    null,
                    null,
                    null,
                    "1",
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
            addressDataManager.WhenToldTo(x => x.FindPostalAddressFromAddressValues(Param.IsAny<PostalAddressModel>())).Return(existingAddresses);

            //link address to client
            serviceMessageCollection = SystemMessageCollection.Empty();
            serviceCommandRouter.WhenToldTo(
                x => x.HandleCommand(Param.IsAny<AddClientAddressCommand>(), serviceRequestMetaData))
                .Return(serviceMessageCollection);

            //get client address linked key
            linkedClientAddressKey = 1234;
            linkedKeyManager.WhenToldTo(x => x.RetrieveLinkedKey(Param.IsAny<Guid>())).Return(linkedClientAddressKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_for_an_existing_address = () =>
        {
            addressDataManager.WasToldTo(x => x.FindPostalAddressFromAddressValues(Param.IsAny<PostalAddressModel>()));
        };

        private It should_not_add_a_new_address = () =>
        {
            serviceCommandRouter.WasNotToldTo(x => x.HandleCommand(Param.IsAny<AddPostalAddressCommand>(), serviceRequestMetaData));
        };

        private It should_use_the_existing_address_when_linking_the_address_to_the_client = () =>
        {
            serviceCommandRouter.WasToldTo(
                x =>
                    x.HandleCommand(Arg.Is<AddClientAddressCommand>(y => y.ClientAddressModel.AddressKey == existingAddresses.First().AddressKey),
                        serviceRequestMetaData));
        };

        private It should_use_the_client_provided_when_linking_the_address_to_the_client = () =>
        {
            serviceCommandRouter.WasToldTo(
                x => x.HandleCommand(Arg.Is<AddClientAddressCommand>(y => y.ClientAddressModel.ClientKey == clientKey), serviceRequestMetaData));
        };

        private It should_not_use_the_linked_key_manager_to_retrieve_the_addressKey = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.RetrieveLinkedKey(guid));
        };

        private It should_not_remove_the_linked_key_for_client_address_id = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.DeleteLinkedKey(command.ClientAddressGuid));
        };

        private It should_add_a_street_address_linked_as_postal_address_to_client_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                Arg.Is<PostalAddressLinkedToClientEvent>
                    (y => y.ClientKey == clientKey
                        && y.AddressFormat == postalAddress.AddressFormat
                        && y.BoxNumber == postalAddress.BoxNumber
                        && y.PostalCode == postalAddress.PostalCode),
                linkedClientAddressKey,
                (int)GenericKeyType.LegalEntityAddress,
                Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}
