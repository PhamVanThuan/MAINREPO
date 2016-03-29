using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
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
    public class when_add_client_address_returns_messages : WithCoreFakes
    {
        private static LinkPostalAddressToClientCommandHandler handler;
        private static LinkPostalAddressToClientCommand command;
        private static IAddressDataManager addressDataManager;
        private static PostalAddressModel postalAddressModel;
        private static int clientKey;
        private static IEnumerable<AddressDataModel> existingAddresses;
        private static Guid clientAddressGuid;
        private static ISystemMessageCollection systemMessageCollection;

        private Establish context = () =>
        {
            //set up mock objects
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

            //postal address exists
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
            addressDataManager.WhenToldTo(x => x.FindPostalAddressFromAddressValues(Param.IsAny<PostalAddressModel>())).Return(existingAddresses);

            //link address to client
            systemMessageCollection = SystemMessageCollection.Empty();
            systemMessageCollection.AddMessage(new SystemMessage("Could not link address to client", SystemMessageSeverityEnum.Error));
            serviceCommandRouter.WhenToldTo(
                x => x.HandleCommand(Param.IsAny<AddClientAddressCommand>(), serviceRequestMetaData))
                .Return(systemMessageCollection);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_add_all_the_messages_from_the_addClientAddressCommand_to_the_handler_collection = () =>
        {
            messages.AllMessages.Count().ShouldEqual(systemMessageCollection.AllMessages.Count());
        };

        private It should_not_add_a_street_address_linked_as_postal_address_to_client_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                Param.IsAny<PostalAddressLinkedToClientEvent>(),
                Param.IsAny<int>(),
                Param.IsAny<int>(),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_complete_the_unitofwork_prior_to_returning = () =>
        {
            unitOfWork.WasToldTo(x => x.Complete());
        };
    }
}
