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
    public class when_adding_a_postal_address_returns_warning_messages : WithCoreFakes
    {
        private static LinkPostalAddressToClientCommandHandler handler;
        private static LinkPostalAddressToClientCommand command;
        private static IAddressDataManager addressDataManager;
        private static PostalAddressModel postalAddressModel;
        private static int clientKey, linkedAddressKey;
        private static IEnumerable<AddressDataModel> existingAddresses;
        private static Guid clientAddressGuid, addressGuid;
        private static ISystemMessageCollection systemMessageCollection;
        private static int linkedClientAddressKey;

        private Establish context = () =>
        {
            //set mock objects
            addressDataManager = An<IAddressDataManager>();

            //new up handler
            handler = new LinkPostalAddressToClientCommandHandler(serviceCommandRouter,
                addressDataManager,
                combGuid,
                linkedKeyManager,
                unitOfWorkFactory,
                eventRaiser);

            //new up command
            postalAddressModel = new PostalAddressModel("1131", "", "Wandsbeck", "KZN", "Durban", "3629", AddressFormat.Box);
            clientKey = 3;
            clientAddressGuid = CombGuid.Instance.Generate();
            command = new LinkPostalAddressToClientCommand(postalAddressModel, clientKey, clientAddressGuid);

            //address does not exist
            existingAddresses = Enumerable.Empty<AddressDataModel>();
            addressDataManager.WhenToldTo(x => x.FindPostalAddressFromAddressValues(Param.IsAny<PostalAddressModel>())).Return(existingAddresses);

            //create guid
            addressGuid = CombGuid.Instance.Generate();
            combGuid.WhenToldTo(x => x.Generate()).Return(addressGuid);

            //add address returns warnings
            systemMessageCollection = SystemMessageCollection.Empty();
            systemMessageCollection.AddMessage(new SystemMessage("Warning encountered.", SystemMessageSeverityEnum.Warning));
            serviceCommandRouter.WhenToldTo(
                x => x.HandleCommand<AddPostalAddressCommand>(Param.IsAny<AddPostalAddressCommand>(), serviceRequestMetaData))
                .Return(systemMessageCollection);

            //get linked key
            linkedAddressKey = 10;
            linkedKeyManager.WhenToldTo(x => x.RetrieveLinkedKey(addressGuid)).Return(linkedAddressKey);

            //link address to client
            serviceCommandRouter.WhenToldTo(
                x => x.HandleCommand<AddClientAddressCommand>(Param.IsAny<AddClientAddressCommand>(), serviceRequestMetaData))
                .Return(SystemMessageCollection.Empty());

            //get client address linked key
            linkedClientAddressKey = 1234;
            linkedKeyManager.WhenToldTo(x => x.RetrieveLinkedKey(clientAddressGuid)).Return(linkedClientAddressKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_get_the_key_of_new_postal_address = () =>
        {
            linkedKeyManager.WasToldTo(x => x.RetrieveLinkedKey(addressGuid));
        };

        private It should_remove_the_linked_key_for_the_address = () =>
        {
            linkedKeyManager.WasToldTo(x => x.DeleteLinkedKey(addressGuid));
        };

        private It should_add_the_client_address = () =>
        {
            serviceCommandRouter.WasToldTo(
                x => x.HandleCommand<AddClientAddressCommand>(Param.IsAny<AddClientAddressCommand>(), serviceRequestMetaData));
        };

        private It should_remove_the_linked_key_for_client_address_id = () =>
        {
            linkedKeyManager.WasToldTo(x => x.DeleteLinkedKey(addressGuid));
        };

        private It should_return_the_warning_messages_from_the_sub_command = () =>
        {
            messages.WarningMessages().First().Message.ShouldEqual("Warning encountered.");
        };

        private It should_add_a_street_address_linked_as_postal_address_to_client_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                Arg.Is<PostalAddressLinkedToClientEvent>
                    (y => y.ClientKey == clientKey
                        && y.AddressFormat == postalAddressModel.AddressFormat
                        && y.BoxNumber == postalAddressModel.BoxNumber
                        && y.PostalCode == postalAddressModel.PostalCode),
                linkedClientAddressKey,
                (int)GenericKeyType.LegalEntityAddress,
                Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}
