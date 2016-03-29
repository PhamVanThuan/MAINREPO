using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.AddressDomain.CommandHandlers;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Events;
using SAHL.Services.Interfaces.AddressDomain.Model;

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.LinkFreeTextAddressAsPostalAddress
{
    public class when_linking_client_address_returns_error_message : WithCoreFakes
    {
        private static LinkFreeTextAddressAsPostalAddressToClientCommandHandler handler;
        private static LinkFreeTextAddressAsPostalAddressToClientCommand command;
        private static IAddressDataManager addressDataManager;
        private static FreeTextAddressModel freeTextAddressModel;
        private static int clientKey;
        private static int linkedAddressKey, linkedClientAddressKey;
        private static Guid addressGuid;
        private static ISystemMessageCollection serviceMessageCollection;

        private Establish context = () =>
        {
            addressDataManager = An<IAddressDataManager>();

            freeTextAddressModel = new FreeTextAddressModel(AddressFormat.FreeText, "42 Wallaby Way", "Sydney", "", "", "", "Australia");
            var clientAddressGuid = CombGuid.Instance.Generate();
            addressGuid = CombGuid.Instance.Generate();
            clientKey = 1;
            linkedAddressKey = 12;

            command = new LinkFreeTextAddressAsPostalAddressToClientCommand(freeTextAddressModel, clientKey, clientAddressGuid);
            handler = new LinkFreeTextAddressAsPostalAddressToClientCommandHandler(serviceCommandRouter,
                addressDataManager,
                combGuid,
                linkedKeyManager,
                unitOfWorkFactory,
                eventRaiser);

            combGuid.WhenToldTo(x => x.Generate()).Return(addressGuid);

            linkedKeyManager.WhenToldTo(x => x.RetrieveLinkedKey(addressGuid)).Return(linkedAddressKey);
            serviceMessageCollection = SystemMessageCollection.Empty();
            serviceMessageCollection.AddMessage(new SystemMessage("Could not link address", SystemMessageSeverityEnum.Error));
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param.IsAny<AddClientAddressCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                .Return(serviceMessageCollection);
            linkedClientAddressKey = 1234;
            linkedKeyManager.WhenToldTo(x => x.RetrieveLinkedKey(clientAddressGuid)).Return(linkedClientAddressKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_return_the_error_messages_from_the_sub_command = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual(serviceMessageCollection.ErrorMessages().First().Message);
        };

        private It should_not_raise_a_free_text_address_as_postal_address_linked_to_client_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(
                Param.IsAny<DateTime>(),
                Param.IsAny<FreeTextPostalAddressLinkedToClientEvent>(),
                linkedClientAddressKey,
                (int)GenericKeyType.LegalEntityAddress,
                Param.IsAny<IServiceRequestMetadata>()
                ));
        };
    }
}
