using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
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

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.LinkFreeTextAddressAsResidentialAddress
{
    public class when_adding_address_returns_error_message : WithCoreFakes
    {
        private static LinkFreeTextAddressAsResidentialAddressToClientCommandHandler handler;
        private static LinkFreeTextAddressAsResidentialAddressToClientCommand command;
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

            command = new LinkFreeTextAddressAsResidentialAddressToClientCommand(freeTextAddressModel, clientKey, clientAddressGuid);
            handler = new LinkFreeTextAddressAsResidentialAddressToClientCommandHandler(serviceCommandRouter,
                addressDataManager,
                combGuid,
                linkedKeyManager,
                unitOfWorkFactory,
                eventRaiser);

            combGuid.WhenToldTo(x => x.Generate()).Return(addressGuid);

            linkedKeyManager.WhenToldTo(x => x.RetrieveLinkedKey(addressGuid)).Return(linkedAddressKey);
            serviceMessageCollection = SystemMessageCollection.Empty();
            serviceMessageCollection.AddMessage(new SystemMessage("Could not add address", SystemMessageSeverityEnum.Error));
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param.IsAny<AddFreeTextAddressCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                .Return(serviceMessageCollection);
            linkedClientAddressKey = 1234;
            linkedKeyManager.WhenToldTo(x => x.RetrieveLinkedKey(clientAddressGuid)).Return(linkedClientAddressKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_if_the_address_exists = () =>
        {
            addressDataManager.WasToldTo(x => x.FindAddressFromFreeTextAddress(Param.IsAny<FreeTextAddressModel>()));
        };

        private It should_try_add_a_new_free_text_address = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Arg.Is<AddFreeTextAddressCommand>(
                y => y.AddressId == addressGuid &&
                    y.FreeTextAddress.FreeText1 == freeTextAddressModel.FreeText1 &&
                    y.FreeTextAddress.FreeText2 == freeTextAddressModel.FreeText2 &&
                    y.FreeTextAddress.FreeText3 == freeTextAddressModel.FreeText3 &&
                    y.FreeTextAddress.FreeText4 == freeTextAddressModel.FreeText4 &&
                    y.FreeTextAddress.Country == freeTextAddressModel.Country &&
                    y.FreeTextAddress.FreeText5 == freeTextAddressModel.FreeText5),
                serviceRequestMetaData));
        };

        private It should_return_the_error_messages_from_the_sub_command = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual(serviceMessageCollection.ErrorMessages().First().Message);
        };

        private It should_not_link_the_client_to_the_new_address = () =>
        {
            serviceCommandRouter.WasNotToldTo(x => x.HandleCommand(Param.IsAny<AddClientAddressCommand>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_not_raise_a_free_text_address_as_residential_address_linked_to_client_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(
                Param.IsAny<DateTime>(),
                Param.IsAny<FreeTextResidentialAddressLinkedToClientEvent>(),
                linkedClientAddressKey,
                (int)GenericKeyType.LegalEntityAddress,
                Param.IsAny<IServiceRequestMetadata>()
                )
                );
        };

        private It should_complete_the_unitofwork_prior_to_returning_messages = () =>
        {
            unitOfWork.WasToldTo(x => x.Complete());
        };
    }
}
