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
    public class when_adding_a_street_address_returns_error_messages : WithCoreFakes
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

        private Establish context = () =>
        {
            //Implement interfaces
            addressDataManager = An<IAddressDataManager>();

            //Setup parameters & models
            clientKey = 5;
            addressGuid = CombGuid.Instance.Generate();
            clientAddressGuid = CombGuid.Instance.Generate();
            existingAddress = Enumerable.Empty<AddressDataModel>();
            streetAddressModel = new StreetAddressModel("", "", "", "7", "Maryland Avenue", "Durban North", "Durban", "Kwazulu-Natal", "4051");

            //Setup message collection
            systemMessageCollection = SystemMessageCollection.Empty();
            systemMessageCollection.AddMessage(new SystemMessage("Error encountered.", SystemMessageSeverityEnum.Error));

            //Command & Handler
            handler = new LinkStreetAddressAsResidentialAddressToClientCommandHandler(serviceCommandRouter,
                addressDataManager,
                combGuid,
                linkedKeyManager,
                unitOfWorkFactory,
                eventRaiser);
            command = new LinkStreetAddressAsResidentialAddressToClientCommand(streetAddressModel, clientKey, clientAddressGuid);

            //Handle find address call and return address
            addressDataManager.WhenToldTo(x => x.FindAddressFromStreetAddress(Param.IsAny<StreetAddressModel>())).Return(existingAddress);

            //Handle client address call and return error/warning messages
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param.IsAny<AddStreetAddressCommand>(), serviceRequestMetaData))
                .Return(systemMessageCollection);

            //Return the generated guid
            combGuid.WhenToldTo(x => x.Generate()).Return(addressGuid);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_return_the_error_messages_from_the_sub_command = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Error encountered.");
        };

        private It should_not_add_the_client_address = () =>
        {
            serviceCommandRouter.WasNotToldTo(
                x => x.HandleCommand<AddClientAddressCommand>(Param.IsAny<AddClientAddressCommand>(), serviceRequestMetaData));
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
