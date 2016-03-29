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

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.LinkStreetAddressAsPostalAddressToClient
{
    public class when_linking_a_street_address_that_is_in_our_system : WithCoreFakes
    {
        private static IAddressDataManager addressDataService;
        private static LinkStreetAddressAsPostalAddressToClientCommandHandler handler;
        private static LinkStreetAddressAsPostalAddressToClientCommand command;
        private static StreetAddressModel streetAddressModel;
        private static int clientKey;
        private static IEnumerable<AddressDataModel> addresses;
        private static Guid clientAddressGuid;
        private static ISystemMessageCollection serviceMessageCollection;
        private static Guid generatedGuid;
        private static int linkedClientAddressKey;

        private Establish context = () =>
        {
            generatedGuid = Guid.NewGuid();

            //Implement interfaces
            addressDataService = An<IAddressDataManager>();
            combGuid.WhenToldTo(x => x.Generate()).Return(generatedGuid);

            //Setup parameters & models
            streetAddressModel = new StreetAddressModel("", "", "", "7", "Maryland Avenue", "Durban North", "Durban", "Kwazulu-Natal", "4051");
            addresses = new[]
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
            clientAddressGuid = CombGuid.Instance.Generate();
            clientKey = 1;

            //Command & Handler
            command = new LinkStreetAddressAsPostalAddressToClientCommand(streetAddressModel, clientKey, clientAddressGuid);
            handler = new LinkStreetAddressAsPostalAddressToClientCommandHandler(serviceCommandRouter,
                addressDataService,
                combGuid,
                linkedKeyManager,
                unitOfWorkFactory,
                eventRaiser);

            //Handle find address call and return address
            addressDataService.WhenToldTo(x => x.FindAddressFromStreetAddress(Param.IsAny<StreetAddressModel>())).Return(addresses);

            //Handle add client address call and return error/warning messages
            serviceMessageCollection = SystemMessageCollection.Empty();
            serviceCommandRouter.WhenToldTo(
                x => x.HandleCommand(Param.IsAny<AddClientAddressCommand>(), serviceRequestMetaData))
                .Return(serviceMessageCollection);

            //get linked client address key
            linkedClientAddressKey = 1234;
            linkedKeyManager.WhenToldTo(x => x.RetrieveLinkedKey(clientAddressGuid)).Return(linkedClientAddressKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_if_the_street_address_already_exists = () =>
        {
            addressDataService.WasToldTo(x => x.FindAddressFromStreetAddress(Param.IsAny<StreetAddressModel>()));
        };

        private It should_not_add_a_new_address = () =>
        {
            serviceCommandRouter.WasNotToldTo(
                x => x.HandleCommand(Param.IsAny<AddStreetAddressCommand>(), serviceRequestMetaData));
        };

        private It should_use_the_existing_address_when_creating_the_client_link = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Arg.Is<AddClientAddressCommand>(
                y => y.ClientAddressModel.AddressKey == addresses.First().AddressKey
                    && y.ClientAddressModel.ClientKey == clientKey
                    && y.ClientAddressModel.AddressType == AddressType.Postal),
                serviceRequestMetaData));
        };

        private It should_not_use_the_linked_key_manager_to_retrieve_the_addressKey = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.RetrieveLinkedKey(generatedGuid));
        };

        private It should_not_remove_the_linked_key_for_client_address_id = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.DeleteLinkedKey(command.ClientAddressGuid));
        };

        private It should_retrieve_the_client_address_key_for_the_event = () =>
        {
            linkedKeyManager.WasToldTo(x => x.RetrieveLinkedKey(command.ClientAddressGuid));
        };

        private It should_raise_a_street_address_as_postal_address_linked_to_client_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                Arg.Is<PostalStreetAddressLinkedToClientEvent>
                    (y => y.ClientKey == command.ClientKey && y.BuildingNumber == command.StreetAddressModel.BuildingNumber),
                linkedClientAddressKey,
                (int)GenericKeyType.LegalEntityAddress,
                Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}
