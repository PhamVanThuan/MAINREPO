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

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.LinkStreetAddressAsResidentialAddressToClient
{
    public class when_linking_a_street_address_not_in_the_system : WithCoreFakes
    {
        private static LinkStreetAddressAsResidentialAddressToClientCommandHandler handler;
        private static IAddressDataManager addressDataManager;
        private static LinkStreetAddressAsResidentialAddressToClientCommand command;
        private static int clientKey, expectedAddressKey;
        private static StreetAddressModel streetAddress;
        private static Guid addressGuid, clientAddressGuid;
        private static IEnumerable<AddressDataModel> existingAddress;
        private static int linkedClientAddressKey;

        private Establish context = () =>
        {
            //Implement interfaces
            addressDataManager = An<IAddressDataManager>();

            //Setup parameters & models
            clientKey = 6;
            expectedAddressKey = 199;
            addressGuid = CombGuid.Instance.Generate();
            clientAddressGuid = CombGuid.Instance.Generate();
            existingAddress = Enumerable.Empty<AddressDataModel>();
            streetAddress = new StreetAddressModel("", "", "", "12", "Clement Stott Rd", "Bothas Hill", "Bothas Hill", "Kwazulu Natal", "3660");

            //Setup message collection

            //Command & Handler
            handler = new LinkStreetAddressAsResidentialAddressToClientCommandHandler(serviceCommandRouter,
                addressDataManager,
                combGuid,
                linkedKeyManager,
                unitOfWorkFactory,
                eventRaiser);
            command = new LinkStreetAddressAsResidentialAddressToClientCommand(streetAddress, clientKey, clientAddressGuid);

            //Handle find address call and return address
            addressDataManager.WhenToldTo(x => x.FindAddressFromStreetAddress(Param.IsAny<StreetAddressModel>())).Return(existingAddress);

            //Handle street address call and return error/warning messages
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param.IsAny<AddStreetAddressCommand>(), serviceRequestMetaData))
                .Return(SystemMessageCollection.Empty());

            //Return the generated guid
            combGuid.WhenToldTo(x => x.Generate()).Return(addressGuid);

            //Use KeyManager to retrieve addresskey
            linkedKeyManager.WhenToldTo(x => x.RetrieveLinkedKey(addressGuid)).Return(expectedAddressKey);

            //Handle client address call and return error/warning messages
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param.IsAny<AddClientAddressCommand>(), serviceRequestMetaData))
                .Return(SystemMessageCollection.Empty());

            //get linked client address key
            linkedClientAddressKey = 1234;
            linkedKeyManager.WhenToldTo(x => x.RetrieveLinkedKey(clientAddressGuid)).Return(linkedClientAddressKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_if_address_is_already_in_the_system = () =>
        {
            addressDataManager.WasToldTo(x => x.FindAddressFromStreetAddress(Param.IsAny<StreetAddressModel>()));
        };

        private It should_add_the_street_address = () =>
        {
            serviceCommandRouter.WasToldTo(
                x => x.HandleCommand(Param.IsAny<AddStreetAddressCommand>(), serviceRequestMetaData));
        };

        private It should_get_the_address_key_of_newly_created_entry = () =>
        {
            linkedKeyManager.WasToldTo(x => x.RetrieveLinkedKey(addressGuid));
        };

        private It should_link_client_to_address = () =>
        {
            serviceCommandRouter.WasToldTo(
                x => x.HandleCommand(Param.IsAny<AddClientAddressCommand>(), serviceRequestMetaData));
        };

        private It should_link_the_address_to_the_clientaddress_guid = () =>
        {
            serviceCommandRouter.WasToldTo(
                x => x.HandleCommand(Arg.Is<AddClientAddressCommand>(y => y.ClientAddressGuid == clientAddressGuid), serviceRequestMetaData));
        };

        private It should_rais_a_street_address_as_residential_address_linked_to_client_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                Arg.Is<ResidentialStreetAddressLinkedToClientEvent>
                    (y => y.StreetName == command.StreetAddressModel.StreetName && y.ClientKey == command.ClientKey),
                Param.IsAny<int>(),
                (int)GenericKeyType.LegalEntityAddress,
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_remove_the_address_guid = () =>
        {
            linkedKeyManager.WasToldTo(x => x.DeleteLinkedKey(addressGuid));
        };

        private It should_not_remove_the_clientaddress_guid = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.DeleteLinkedKey(clientAddressGuid));
        };

        private It should_return_an_empty_system_messages_collection = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };

        private It should_should_raise_a_street_address_as_postal_address_linked_to_client_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                Arg.Is<ResidentialStreetAddressLinkedToClientEvent>
                    (y => y.StreetName == command.StreetAddressModel.StreetName && y.ClientKey == command.ClientKey),
                linkedClientAddressKey,
                (int)GenericKeyType.LegalEntityAddress,
                Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}
