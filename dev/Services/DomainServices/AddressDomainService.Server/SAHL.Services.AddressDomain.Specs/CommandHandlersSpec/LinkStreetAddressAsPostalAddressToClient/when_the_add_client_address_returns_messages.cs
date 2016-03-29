using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
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
    public class when__the_add_client_address_returns_messages : WithCoreFakes
    {
        private static LinkStreetAddressAsPostalAddressToClientCommandHandler handler;
        private static LinkStreetAddressAsPostalAddressToClientCommand command;
        private static IAddressDataManager addressDataManager;
        private static int clientKey;
        private static StreetAddressModel streetAddressModel;
        private static Guid clientAddressGuid;
        private static ISystemMessageCollection serviceMessageCollection;
        private static IEnumerable<AddressDataModel> existingAddresses;

        private Establish context = () =>
        {
            //Implement interfaces
            addressDataManager = An<IAddressDataManager>();

            //Setup parameters & models
            clientAddressGuid = CombGuid.Instance.Generate();
            streetAddressModel = new StreetAddressModel("", "", "", "7", "Maryland Avenue", "Durban North", "Durban", "Kwazulu-Natal", "4051");
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
            clientKey = 3;

            //Setup message collection
            serviceMessageCollection = SystemMessageCollection.Empty();
            serviceMessageCollection.AddMessage(new SystemMessage("Could not link address to client", SystemMessageSeverityEnum.Error));

            //Command & Handler
            handler = new LinkStreetAddressAsPostalAddressToClientCommandHandler(serviceCommandRouter,
                addressDataManager,
                combGuid,
                linkedKeyManager,
                unitOfWorkFactory,
                eventRaiser);
            command = new LinkStreetAddressAsPostalAddressToClientCommand(streetAddressModel, clientKey, clientAddressGuid);

            //Does client exist

            //Handle find address call and return address
            addressDataManager.WhenToldTo(x => x.FindAddressFromStreetAddress(Param.IsAny<StreetAddressModel>())).Return(existingAddresses);

            //Handle add client address call and return error/warning messages
            serviceCommandRouter.WhenToldTo(
                x => x.HandleCommand(Param.IsAny<AddClientAddressCommand>(), serviceRequestMetaData))
                .Return(serviceMessageCollection);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_add_all_the_messages_from_the_addClientAddressCommand_to_the_handler_collection = () =>
        {
            messages.AllMessages.Count().ShouldEqual(serviceMessageCollection.AllMessages.Count());
        };

        private It should_contain_the_messages_for_each_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual(serviceMessageCollection.ErrorMessages().First().Message);
        };

        private It should_not_raise_a_street_address_as_postal_address_linked_to_client_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                Param.IsAny<PostalStreetAddressLinkedToClientEvent>(),
                Param.IsAny<int>(),
                Param.IsAny<int>(),
                Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}
