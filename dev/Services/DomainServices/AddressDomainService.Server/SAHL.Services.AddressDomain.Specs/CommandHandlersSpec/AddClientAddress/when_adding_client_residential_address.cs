using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Identity;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.Services.AddressDomain.CommandHandlers.Internal;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.Linq;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.AddClientAddress
{
    public class when_adding_client_residential_address : WithCoreFakes
    {
        private static AddClientAddressCommand command;
        private static AddClientAddressCommandHandler handler;
        private static int clientKey, addressKey, clientAddressKey;
        private static AddressType addressType;
        private static Guid clientAddressGuid;
        private static IAddressDataManager addressDataManager;
        private static ClientAddressModel clientAddress;
        private static IDomainRuleManager<ClientAddressModel> domainRuleManager;

        private Establish context = () =>
        {
            clientKey = 1024;
            addressKey = 30398;
            addressType = AddressType.Residential;
            clientAddressKey = 954;
            clientAddressGuid = combGuid.Generate();
            domainRuleManager = An<IDomainRuleManager<ClientAddressModel>>();
            addressDataManager = An<IAddressDataManager>();
            addressDataManager.WhenToldTo(a => a.SaveClientAddress(Param.IsAny<ClientAddressModel>())).Return(clientAddressKey);

            clientAddress = new ClientAddressModel(clientKey, addressKey, addressType);
            command = new AddClientAddressCommand(clientAddress, clientAddressGuid);
            handler = new AddClientAddressCommandHandler(addressDataManager, linkedKeyManager, domainRuleManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_execute_the_registered_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), command.ClientAddressModel));
        };

        private It should_adds_the_given_client_residential_address = () =>
        {
            addressDataManager.WasToldTo(x => x.SaveClientAddress(
                Arg.Is<ClientAddressModel>(c => c.AddressKey == addressKey && c.AddressType == addressType && c.ClientKey == clientKey)));
        };

        private It should_links_the_client_address_to_client_guid = () =>
        {
            linkedKeyManager.WasToldTo(l => l.LinkKeyToGuid(clientAddressKey, clientAddressGuid));
        };

        private It should_not_add_any_messages = () =>
        {
            messages.AllMessages.Count().ShouldEqual(0);
        };
    }
}