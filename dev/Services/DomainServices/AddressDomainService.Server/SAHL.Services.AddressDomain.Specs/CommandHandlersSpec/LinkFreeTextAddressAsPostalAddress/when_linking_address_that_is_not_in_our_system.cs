using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.AddressDomain.CommandHandlers;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Events;
using SAHL.Services.Interfaces.AddressDomain.Model;

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.LinkFreeTextAddressAsPostalAddress
{
    public class when_linking_address_that_is_not_in_our_system : WithCoreFakes
    {
        private static LinkFreeTextAddressAsPostalAddressToClientCommandHandler handler;
        private static LinkFreeTextAddressAsPostalAddressToClientCommand command;
        private static IAddressDataManager addressDataManager;
        private static FreeTextAddressModel freeTextAddressModel;
        private static int clientKey;
        private static int linkedAddressKey, linkedClientAddressKey;
        private static Guid addressGuid;

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

        private It should_add_a_new_free_text_address = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand<AddFreeTextAddressCommand>(Arg.Is<AddFreeTextAddressCommand>(
                y => y.AddressId == addressGuid &&
                    y.FreeTextAddress.FreeText1 == freeTextAddressModel.FreeText1 &&
                    y.FreeTextAddress.FreeText2 == freeTextAddressModel.FreeText2 &&
                    y.FreeTextAddress.FreeText3 == freeTextAddressModel.FreeText3 &&
                    y.FreeTextAddress.FreeText4 == freeTextAddressModel.FreeText4 &&
                    y.FreeTextAddress.Country == freeTextAddressModel.Country &&
                    y.FreeTextAddress.FreeText5 == freeTextAddressModel.FreeText5),
                serviceRequestMetaData));
        };

        private It should_get_the_key_of_new_address = () =>
        {
            linkedKeyManager.WasToldTo(x => x.RetrieveLinkedKey(addressGuid));
        };

        private It should_remove_the_linked_key_for_the_address = () =>
        {
            linkedKeyManager.WasToldTo(x => x.DeleteLinkedKey(addressGuid));
        };

        private It should_link_the_client_to_the_new_address_as_their_postal_address = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand<AddClientAddressCommand>(Arg.Is<AddClientAddressCommand>(
                y => y.ClientAddressModel.ClientKey == clientKey && y.ClientAddressModel.AddressKey == linkedAddressKey &&
                    y.ClientAddressModel.AddressType == AddressType.Postal),
                serviceRequestMetaData));
        };

        private It should_not_remove_the_linked_key_for_client_address_id = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.DeleteLinkedKey(command.ClientAddressGuid));
        };

        private It should_raise_a_free_text_address_as_residential_address_linked_to_client_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                Arg.Is<FreeTextPostalAddressLinkedToClientEvent>
                    (y => y.ClientKey == command.ClientKey &&
                        doesEventAddressFreeTextPropertiesMatch(y, freeTextAddressModel) &&
                        y.AddressFormat == freeTextAddressModel.AddressFormat &&
                        y.ClientAddressKey == linkedClientAddressKey),
                linkedClientAddressKey,
                (int)GenericKeyType.LegalEntityAddress,
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private static bool doesEventAddressFreeTextPropertiesMatch(FreeTextPostalAddressLinkedToClientEvent sourceAddress,
            FreeTextAddressModel addressToMatch)
        {
            return (
                sourceAddress.FreeText1 == addressToMatch.FreeText1 &&
                    sourceAddress.FreeText2 == addressToMatch.FreeText2 &&
                    sourceAddress.FreeText3 == addressToMatch.FreeText3 &&
                    sourceAddress.FreeText4 == addressToMatch.FreeText4 &&
                    sourceAddress.FreeText5 == addressToMatch.FreeText5
                );
        }
    }
}
