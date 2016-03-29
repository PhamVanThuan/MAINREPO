using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.AddressDomain.CommandHandlers.Internal;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.AddClientAddress
{
    public class when_rules_fail : WithCoreFakes
    {
        private static AddClientAddressCommandHandler handler;
        private static AddClientAddressCommand command;
        private static IDomainRuleManager<ClientAddressModel> domainRuleManager;
        private static ClientAddressModel clientAddressModel;
        private static Guid clientAddressGuid;
        private static IAddressDataManager addressDataManager;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            clientAddressModel = new ClientAddressModel(1, 2, Core.BusinessModel.Enums.AddressType.Postal);
            command = new AddClientAddressCommand(clientAddressModel, clientAddressGuid);
            addressDataManager = An<IAddressDataManager>();
            domainRuleManager = An<IDomainRuleManager<ClientAddressModel>>();
            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), command.ClientAddressModel))
                .Callback<ISystemMessageCollection>(y => y.AddMessage(new SystemMessage("Rule failure message.", SystemMessageSeverityEnum.Error)));
            clientAddressGuid = CombGuid.Instance.Generate();
            handler = new AddClientAddressCommandHandler(addressDataManager, linkedKeyManager, domainRuleManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_not_add_the_client_address = () =>
        {
            addressDataManager.WasNotToldTo(x => x.SaveClientAddress(Param.IsAny<ClientAddressModel>()));
        };

        private It should_not_use_the_linkedKeyManager_to_link_the_clientAddressKey_and_guid = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.LinkKeyToGuid(Param.IsAny<int>(), Param.IsAny<Guid>()));
        };

        private It should_return_the_rule_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Rule failure message.");
        };
    }
}
