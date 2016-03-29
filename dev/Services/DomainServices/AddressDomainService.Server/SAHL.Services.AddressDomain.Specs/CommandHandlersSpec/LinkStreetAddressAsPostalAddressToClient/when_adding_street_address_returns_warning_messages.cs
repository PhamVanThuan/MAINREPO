using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Identity;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.AddressDomain.CommandHandlers;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Events;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core.Data;

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.LinkStreetAddressAsPostalAddressToClient
{
    public class when_adding_street_address_returns_warning_messages : WithCoreFakes
    {
        private static LinkStreetAddressAsPostalAddressToClientCommandHandler handler;
        private static LinkStreetAddressAsPostalAddressToClientCommand command;
        private static IAddressDataManager addressDataManager;
        private static StreetAddressModel streetAddressModel;
        private static int clientKey;
        private static int linkedAddressKey, linkedClientAddressKey;
        private static Guid addressGuid;
        private static Guid clientAddressGuid;
        private static IEnumerable<AddressDataModel> existingAddresses;
        private static ISystemMessageCollection serviceMessageCollection;


        private Establish context = () =>
        {
            //Implement interfaces
            addressDataManager = An<IAddressDataManager>();

            //Setup parameters & models
            streetAddressModel = new StreetAddressModel("", "", "", "7", "Maryland Avenue", "Durban North", "Durban", "Kwazulu-Natal", "4051");
            existingAddresses = new AddressDataModel[] { };
            clientAddressGuid = CombGuid.Instance.Generate();
            clientKey = 1;
            linkedAddressKey = 12;
            addressGuid = CombGuid.Instance.Generate();

            //Setup message collection
            serviceMessageCollection = SystemMessageCollection.Empty();
            serviceMessageCollection.AddMessage(new SystemMessage("Warning encountered.", SystemMessageSeverityEnum.Warning));

            //Command & Handler
            command = new LinkStreetAddressAsPostalAddressToClientCommand(streetAddressModel, clientKey, clientAddressGuid);
            handler = new LinkStreetAddressAsPostalAddressToClientCommandHandler(serviceCommandRouter, addressDataManager, combGuid, linkedKeyManager, unitOfWorkFactory, eventRaiser);

            //Handle find address call and return address
            addressDataManager.WhenToldTo(x => x.FindAddressFromStreetAddress(Param.IsAny<StreetAddressModel>())).Return(existingAddresses);

            //Handle street address call and return error/warning messages
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param.IsAny<AddStreetAddressCommand>(), serviceRequestMetaData)).Return(serviceMessageCollection);

            //Return the generated guid
            combGuid.WhenToldTo(x => x.Generate()).Return(addressGuid);

            //Use KeyManager to retrieve addresskey
            linkedKeyManager.WhenToldTo(x => x.RetrieveLinkedKey(addressGuid)).Return(linkedAddressKey);

            //Handle add client address call and return error/warning messages
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param.IsAny<AddClientAddressCommand>(), serviceRequestMetaData)).Return(SystemMessageCollection.Empty());

            //get linked client address key
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

        private It should_return_the_error_messages_from_the_sub_command = () =>
        {
            messages.WarningMessages().First().Message.ShouldEqual("Warning encountered.");
        };

        private It should_add_the_client_address = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param.IsAny<AddClientAddressCommand>(), serviceRequestMetaData));
        };

        private It should_should_raise_a_street_address_as_postal_address_linked_to_client_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<PostalStreetAddressLinkedToClientEvent>
                (y => y.ClientKey == command.ClientKey && y.BuildingNumber == command.StreetAddressModel.BuildingNumber),
                    linkedClientAddressKey, (int)GenericKeyType.LegalEntityAddress, Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_remove_the_linked_key_for_client_address_id = () =>
        {
            linkedKeyManager.WasToldTo(x => x.DeleteLinkedKey(addressGuid));
        };
    }
}