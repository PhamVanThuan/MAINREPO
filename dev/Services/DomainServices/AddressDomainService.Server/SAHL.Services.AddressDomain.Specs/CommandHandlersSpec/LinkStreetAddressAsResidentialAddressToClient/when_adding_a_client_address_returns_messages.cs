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

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.LinkStreetAddressAsResidentialAddressToClient
{
    internal class when_adding_a_client_address_returns_messages : WithCoreFakes
    {
        private static LinkStreetAddressAsResidentialAddressToClientCommandHandler handler;
        private static LinkStreetAddressAsResidentialAddressToClientCommand command;
        private static IAddressDataManager addressDataManager;
        private static int clientKey;
        private static StreetAddressModel streetAddressModel;
        private static Guid clientAddressGuid;
        private static ISystemMessageCollection messageCollection;
        private static IEnumerable<AddressDataModel> existingAddresses;

        private Establish context = () =>
        {
            //Implement interfaces
            addressDataManager = An<IAddressDataManager>();

            //Setup parameters & models
            clientKey = 1;
            clientAddressGuid = CombGuid.Instance.Generate();
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
            streetAddressModel = new StreetAddressModel("", "", "", "7", "Maryland Avenue", "Durban North", "Durban", "Kwazulu-Natal", "4051");

            //Setup message collection
            messageCollection = SystemMessageCollection.Empty();
            messageCollection.AddMessage(new SystemMessage("Error", SystemMessageSeverityEnum.Error));
            messageCollection.AddMessage(new SystemMessage("Warning", SystemMessageSeverityEnum.Warning));

            //Command & Handler
            handler = new LinkStreetAddressAsResidentialAddressToClientCommandHandler(serviceCommandRouter,
                addressDataManager,
                combGuid,
                linkedKeyManager,
                unitOfWorkFactory,
                eventRaiser);
            command = new LinkStreetAddressAsResidentialAddressToClientCommand(streetAddressModel, clientKey, clientAddressGuid);

            //Does client exist

            //Handle find address call and return address
            addressDataManager.WhenToldTo(x => x.FindAddressFromStreetAddress(Param.IsAny<StreetAddressModel>())).Return(existingAddresses);

            //Handle client address call and return error/warning messages
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param.IsAny<AddClientAddressCommand>(), serviceRequestMetaData))
                .Return(messageCollection);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_add_all_the_messages_from_the_addClientAddressCommand_to_the_handler_collection = () =>
        {
            messages.AllMessages.Count().ShouldEqual(messageCollection.AllMessages.Count());
        };

        private It should_contain_the_messages_for_each_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual(messageCollection.ErrorMessages().First().Message);
        };

        private It should_not_raise_a_street_address_as_residential_address_linked_to_client_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                Param.IsAny<ResidentialStreetAddressLinkedToClientEvent>(),
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
