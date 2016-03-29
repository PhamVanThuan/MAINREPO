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
    public class when_linking_a_street_address_that_is_in_our_system : WithCoreFakes
    {
        private static LinkStreetAddressAsResidentialAddressToClientCommand command;
        private static LinkStreetAddressAsResidentialAddressToClientCommandHandler handler;
        private static IAddressDataManager addressDataManager;
        private static int clientKey, linkedClientAddressKey;
        private static StreetAddressModel streetAddress;
        private static Guid clientAddressGuid;
        private static IEnumerable<AddressDataModel> existingAddresses;

        private Establish context = () =>
        {
            //Implement interfaces
            addressDataManager = An<IAddressDataManager>();
            //Setup parameters & models
            clientKey = 6;
            existingAddresses = new[]
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
            streetAddress = new StreetAddressModel("", "", "", "12", "Clement Stott Rd", "Bothas Hill", "Bothas Hill", "Kwazulu Natal", "3660");
            clientAddressGuid = CombGuid.Instance.Generate();

            //Command & Handler
            command = new LinkStreetAddressAsResidentialAddressToClientCommand(streetAddress, clientKey, clientAddressGuid);
            handler = new LinkStreetAddressAsResidentialAddressToClientCommandHandler(serviceCommandRouter,
                addressDataManager,
                combGuid,
                linkedKeyManager,
                unitOfWorkFactory,
                eventRaiser);

            //Handle find address call and return address
            addressDataManager.WhenToldTo(x => x.FindAddressFromStreetAddress(Param.IsAny<StreetAddressModel>())).Return(existingAddresses);

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

        private It should_check_if_address_already_exists_in_repository = () =>
        {
            addressDataManager.WasToldTo(x => x.FindAddressFromStreetAddress(Param.IsAny<StreetAddressModel>()));
        };

        private It should_link_client_to_existing_address = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Arg.Is<AddClientAddressCommand>(y => y.ClientAddressGuid == clientAddressGuid
                && y.ClientAddressModel.AddressKey == existingAddresses.First().AddressKey),
                serviceRequestMetaData));
        };

        private It should_return_an_empty_system_messages_collection = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };

        private It should_retrieve_the_client_address_key_for_the_event = () =>
        {
            linkedKeyManager.WasToldTo(x => x.RetrieveLinkedKey(command.ClientAddressGuid));
        };

        private It should_only_use_the_retrieve_key_once = () =>
        {
            linkedKeyManager.WasToldTo(x => x.RetrieveLinkedKey(Param.IsAny<Guid>())).OnlyOnce();
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
