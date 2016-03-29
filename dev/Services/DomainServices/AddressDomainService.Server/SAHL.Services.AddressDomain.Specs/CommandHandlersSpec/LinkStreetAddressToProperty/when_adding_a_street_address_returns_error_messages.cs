using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.AddressDomain.CommandHandlers;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Events;
using SAHL.Services.Interfaces.AddressDomain.Model;

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.LinkStreetAddressToProperty
{
    public class when_adding_a_street_address_returns_error_messages : WithCoreFakes
    {
        private static LinkStreetAddressToPropertyCommandHandler handler;
        private static LinkStreetAddressToPropertyCommand command;
        private static IAddressDataManager addressDataManager;
        private static ISystemMessageCollection systemMessageCollection;
        private static int propertyKey, addressKey;
        private static StreetAddressModel streetAddressModel;

        private Establish context = () =>
        {
            //set up mock objects
            addressDataManager = An<IAddressDataManager>();

            //new up handler
            handler = new LinkStreetAddressToPropertyCommandHandler(serviceCommandRouter,
                addressDataManager,
                combGuid,
                linkedKeyManager,
                unitOfWorkFactory,
                eventRaiser);

            //new up command
            streetAddressModel = new StreetAddressModel("", "", "", "7", "Maryland Avenue", "Durban North", "Durban", "Kwazulu-Natal", "4051");
            propertyKey = 15;
            command = new LinkStreetAddressToPropertyCommand(streetAddressModel, propertyKey);

            //property exists

            //address does not exists
            addressDataManager.WhenToldTo(x => x.FindAddressFromStreetAddress(Param.IsAny<StreetAddressModel>()))
                .Return(new List<AddressDataModel>() { });

            //create guid
            addressKey = 1234;
            linkedKeyManager.WhenToldTo(l => l.RetrieveLinkedKey(Param.IsAny<Guid>())).Return(addressKey);

            //add address fails
            systemMessageCollection = SystemMessageCollection.Empty();
            systemMessageCollection.AddMessage(new SystemMessage("Error encountered.", SystemMessageSeverityEnum.Error));
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param.IsAny<AddStreetAddressCommand>(), serviceRequestMetaData))
                .Return(systemMessageCollection);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_if_street_address_is_already_on_system = () =>
        {
            addressDataManager.WasToldTo(x => x.FindAddressFromStreetAddress(streetAddressModel));
        };

        private It should_return_the_error_messages_from_the_sub_command = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Error encountered.");
        };

        private It should_not_link_street_address_to_property = () =>
        {
            addressDataManager.WasNotToldTo(a => a.LinkAddressToProperty(command.PropertyKey, addressKey));
        };

        private It should_not_raise_a_street_address_linked_to_property_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                Param.IsAny<StreetAddressLinkedToPropertyEvent>(),
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
