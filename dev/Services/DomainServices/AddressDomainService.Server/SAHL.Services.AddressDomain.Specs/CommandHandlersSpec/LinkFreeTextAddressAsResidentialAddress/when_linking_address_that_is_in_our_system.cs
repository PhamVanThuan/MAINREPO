using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.AddressDomain.CommandHandlers;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Events;
using SAHL.Services.Interfaces.AddressDomain.Model;

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.LinkFreeTextAddressAsResidentialAddress
{
    public class when_linking_address_that_is_in_our_system : WithCoreFakes
    {
        private static LinkFreeTextAddressAsResidentialAddressToClientCommandHandler handler;
        private static LinkFreeTextAddressAsResidentialAddressToClientCommand command;
        private static IAddressDataManager addressDataManager;
        private static FreeTextAddressModel freeTextAddressModel;
        private static int clientKey;
        private static int addressKey, linkedClientAddressKey;
        private static Guid addressGuid;
        private static IEnumerable<AddressDataModel> existingAddresses;

        private Establish context = () =>
        {
            addressDataManager = An<IAddressDataManager>();
            clientKey = 1;
            addressKey = 12;
            freeTextAddressModel = new FreeTextAddressModel(AddressFormat.FreeText, "42 Wallaby Way", "Sydney", "", "", "", "Australia");
            existingAddresses = new[]
            {
                new AddressDataModel(addressKey,
                    1,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    "42 Wallaby Way",
                    "Sydney",
                    "",
                    "",
                    "")
            };
            var clientAddressGuid = CombGuid.Instance.Generate();
            addressGuid = CombGuid.Instance.Generate();

            command = new LinkFreeTextAddressAsResidentialAddressToClientCommand(freeTextAddressModel, clientKey, clientAddressGuid);
            handler = new LinkFreeTextAddressAsResidentialAddressToClientCommandHandler(serviceCommandRouter,
                addressDataManager,
                combGuid,
                linkedKeyManager,
                unitOfWorkFactory,
                eventRaiser);

            addressDataManager.WhenToldTo(x => x.FindAddressFromFreeTextAddress(Param.IsAny<FreeTextAddressModel>())).Return(existingAddresses);

            combGuid.WhenToldTo(x => x.Generate()).Return(addressGuid);

            linkedKeyManager.WhenToldTo(x => x.RetrieveLinkedKey(addressGuid)).Return(addressKey);

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

        private It should_not_add_a_new_street_address = () =>
        {
            serviceCommandRouter.WasNotToldTo(x => x.HandleCommand(Arg.Any<AddFreeTextAddressCommand>(), serviceRequestMetaData));
        };

        private It should_link_the_client_to_the_new_address = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Arg.Is<AddClientAddressCommand>(
                y => y.ClientAddressModel.ClientKey == clientKey && y.ClientAddressModel.AddressKey == addressKey &&
                    y.ClientAddressModel.AddressType == AddressType.Residential),
                serviceRequestMetaData));
        };

        private It should_not_remove_the_linked_key_for_client_address_id = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.DeleteLinkedKey(command.ClientAddressGuid));
        };

        private It should_raise_a_free_text_address_as_residential_address_linked_to_client_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                Arg.Is<FreeTextResidentialAddressLinkedToClientEvent>
                    (y => y.ClientKey == command.ClientKey &&
                        y.FreeText1 == freeTextAddressModel.FreeText1 &&
                        y.FreeText2 == freeTextAddressModel.FreeText2 &&
                        y.FreeText3 == freeTextAddressModel.FreeText3 &&
                        y.FreeText4 == freeTextAddressModel.FreeText4 &&
                        y.FreeText5 == freeTextAddressModel.FreeText5 &&
                        y.Country == freeTextAddressModel.Country &&
                        y.AddressFormat == freeTextAddressModel.AddressFormat &&
                        y.ClientAddressKey == linkedClientAddressKey),
                linkedClientAddressKey,
                (int)GenericKeyType.LegalEntityAddress,
                Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}
