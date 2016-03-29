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
    public class when_adding_a_postal_address_returns_error_messages : WithCoreFakes
    {
        private static LinkPostalAddressToClientCommandHandler handler;
        private static LinkPostalAddressToClientCommand command;
        private static IAddressDataManager addressDataManager;
        private static PostalAddressModel postalAddressModel;
        private static int clientKey;
        private static IEnumerable<AddressDataModel> existingAddress;
        private static Guid clientAddressGuid, addressGuid;
        private static ISystemMessageCollection systemMessageCollection, clientMessageCollection;

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

            // new up command
            postalAddressModel = new PostalAddressModel("1131", "", "Wandsbeck", "KZN", "Durban", "3629", AddressFormat.Box);
            clientKey = 3;
            clientAddressGuid = CombGuid.Instance.Generate();
            command = new LinkPostalAddressToClientCommand(postalAddressModel, clientKey, clientAddressGuid);

            //client exists
            clientMessageCollection = SystemMessageCollection.Empty();

            //postal address does not exist
            existingAddress = Enumerable.Empty<AddressDataModel>();
            addressDataManager.WhenToldTo(x => x.FindPostalAddressFromAddressValues(Param.IsAny<PostalAddressModel>())).Return(existingAddress);

            //create guid
            addressGuid = CombGuid.Instance.Generate();
            combGuid.WhenToldTo(x => x.Generate()).Return(addressGuid);

            //add address returns errors
            systemMessageCollection = SystemMessageCollection.Empty();
            systemMessageCollection.AddMessage(new SystemMessage("Error encountered.", SystemMessageSeverityEnum.Error));
            serviceCommandRouter.WhenToldTo(
                x => x.HandleCommand(Param.IsAny<AddPostalAddressCommand>(), serviceRequestMetaData))
                .Return(systemMessageCollection);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_return_the_error_messages_from_the_sub_command = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Error encountered.");
        };

        private It should_not_get_the_key_of_new_postal_address = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.RetrieveLinkedKey(Param.IsAny<Guid>()));
        };

        private It should_not_remove_the_linked_key_for_client_address_id = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.DeleteLinkedKey(Param.IsAny<Guid>()));
        };

        private It should_not_add_the_client_address = () =>
        {
            serviceCommandRouter.WasNotToldTo(
                x => x.HandleCommand(Param.IsAny<AddClientAddressCommand>(), serviceRequestMetaData));
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
