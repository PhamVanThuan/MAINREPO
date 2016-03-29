using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
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
    public class when_linking_to_a_postal_address_that_is_not_in_our_system : WithCoreFakes
    {
        private static LinkPostalAddressToClientCommandHandler handler;
        private static LinkPostalAddressToClientCommand command;
        private static IAddressDataManager addressDataManager;
        private static int linkedAddressKey;
        private static AddressType addressType;
        private static int clientKey;
        private static PostalAddressModel postalAddressModel;
        private static Guid addressGuid;
        private static Guid clientAddressGuid;
        private static IEnumerable<AddressDataModel> existingAddresses;
        private static ISystemMessageCollection systemMessageCollection;
        private static int linkedClientAddressKey;

        private Establish context = () =>
        {
            //set variables
            addressType = AddressType.Postal;

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
            postalAddressModel = new PostalAddressModel("1883", "", "Hillcrest", "Kwazulu Natal", "Hillcrest", "3650", AddressFormat.Box);
            clientKey = 6;
            clientAddressGuid = CombGuid.Instance.Generate();
            command = new LinkPostalAddressToClientCommand(postalAddressModel, clientKey, clientAddressGuid);

            //postal address does not exist
            existingAddresses = Enumerable.Empty<AddressDataModel>();
            addressDataManager.WhenToldTo(x => x.FindPostalAddressFromAddressValues(Param.IsAny<PostalAddressModel>())).Return(existingAddresses);

            //create guid
            addressGuid = CombGuid.Instance.Generate();
            combGuid.WhenToldTo(x => x.Generate()).Return(addressGuid);

            //add address
            systemMessageCollection = SystemMessageCollection.Empty();
            serviceCommandRouter.WhenToldTo(
                x => x.HandleCommand(Param.IsAny<AddPostalAddressCommand>(), serviceRequestMetaData))
                .Return(systemMessageCollection);

            //get linked address key
            linkedAddressKey = 12;
            linkedKeyManager.WhenToldTo(x => x.RetrieveLinkedKey(addressGuid)).Return(linkedAddressKey);

            //link address to client
            serviceCommandRouter.WhenToldTo(
                x => x.HandleCommand(Param.IsAny<AddClientAddressCommand>(), serviceRequestMetaData))
                .Return(systemMessageCollection);

            //get linked client address key
            linkedClientAddressKey = 1234;
            linkedKeyManager.WhenToldTo(x => x.RetrieveLinkedKey(clientAddressGuid)).Return(linkedClientAddressKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_for_an_existing_postal_address = () =>
        {
            addressDataManager.WasToldTo(x => x.FindPostalAddressFromAddressValues(Param.IsAny<PostalAddressModel>()));
        };

        private It should_add_a_new_postal_address = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Arg.Is<AddPostalAddressCommand>
                (y => y.AddressId == addressGuid && y.PostalAddressModel.PostalCode == postalAddressModel.PostalCode),
                serviceRequestMetaData));
        };

        private It should_get_the_key_of_new_postal_address = () =>
        {
            linkedKeyManager.WasToldTo(x => x.RetrieveLinkedKey(addressGuid));
        };

        private It should_link_client_to_address = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Arg.Is<AddClientAddressCommand>
                (y =>
                    y.ClientAddressModel.ClientKey == clientKey && y.ClientAddressModel.AddressKey == linkedAddressKey
                        && y.ClientAddressModel.AddressType == addressType),
                serviceRequestMetaData));
        };

        private It should_not_remove_the_linked_key_for_client_address_id = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.DeleteLinkedKey(command.ClientAddressGuid));
        };

        private It should_add_a_street_address_linked_as_postal_address_to_client_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                Arg.Is<PostalAddressLinkedToClientEvent>
                    (y => y.ClientKey == clientKey
                        && y.AddressFormat == postalAddressModel.AddressFormat
                        && y.BoxNumber == postalAddressModel.BoxNumber
                        && y.PostalCode == postalAddressModel.PostalCode),
                linkedClientAddressKey,
                (int)GenericKeyType.LegalEntityAddress,
                Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}
