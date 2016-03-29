using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
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

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.LinkStreetAddressAsResidentialAddressToClient
{
    public class when_adding_a_street_address_returns_warning_messages : WithCoreFakes
    {
        private static LinkStreetAddressAsResidentialAddressToClientCommandHandler handler;
        private static LinkStreetAddressAsResidentialAddressToClientCommand command;
        private static IAddressDataManager addressDataManager;
        private static Guid clientAddressGuid;
        private static int clientKey;
        private static StreetAddressModel streetAddressModel;
        private static ISystemMessageCollection systemMessageCollection;
        private static IEnumerable<AddressDataModel> existingAddress;
        private static Guid addressGuid;
        private static int linkedClientAddressKey;

        private Establish context = () =>
        {
            //Implement interfaces
            addressDataManager = An<IAddressDataManager>();

            //Setup parameters & models
            addressGuid = CombGuid.Instance.Generate();
            clientAddressGuid = CombGuid.Instance.Generate();
            clientKey = 5;
            existingAddress = Enumerable.Empty<AddressDataModel>();
            streetAddressModel = new StreetAddressModel("", "", "", "7", "Maryland Avenue", "Durban North", "Durban", "Kwazulu-Natal", "4051");

            //Setup message collection
            systemMessageCollection = SystemMessageCollection.Empty();
            systemMessageCollection.AddMessage(new SystemMessage("Warning encountered.", SystemMessageSeverityEnum.Warning));

            //Command & Handler
            handler = new LinkStreetAddressAsResidentialAddressToClientCommandHandler(serviceCommandRouter, addressDataManager, combGuid, linkedKeyManager, unitOfWorkFactory, eventRaiser);
            command = new LinkStreetAddressAsResidentialAddressToClientCommand(streetAddressModel, clientKey, clientAddressGuid);

            //Handle find address call and return address
            addressDataManager.WhenToldTo(x => x.FindAddressFromStreetAddress(Param.IsAny<StreetAddressModel>())).Return(existingAddress);

            //Handle street address call and return error/warning messages
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param.IsAny<AddStreetAddressCommand>(), serviceRequestMetaData)).Return(systemMessageCollection);

            //Return the generated guid
            combGuid.WhenToldTo(x => x.Generate()).Return(addressGuid);
            linkedKeyManager.WhenToldTo(x => x.RetrieveLinkedKey(addressGuid)).Return(12345);

            //Handle client address call and return error/warning messages
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

        private It should_raise_a_street_address_as_residential_address_linked_to_client_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<ResidentialStreetAddressLinkedToClientEvent>
                (y => y.StreetName == command.StreetAddressModel.StreetName && y.ClientKey == command.ClientKey),
                    linkedClientAddressKey, (int)GenericKeyType.LegalEntityAddress, Param.IsAny<IServiceRequestMetadata>()));
        }; 

        private It should_remove_the_linked_key_for_client_address_id = () =>
        {
            linkedKeyManager.WasToldTo(x => x.DeleteLinkedKey(addressGuid));
        };
    }
}