using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.AddressDomain.CommandHandlers.Internal;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.AddressDomain.Rules;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.AddClientAddress
{
    public class when_registering_rules : WithCoreFakes
    {
        private static AddClientAddressCommandHandler handler;
        private static IDomainRuleManager<ClientAddressModel> domainRuleManager;
        private static IAddressDataManager addressDataManager;

        private Establish context = () =>
        {
            addressDataManager = An<IAddressDataManager>();
            domainRuleManager = An<IDomainRuleManager<ClientAddressModel>>();
        };

        private Because of = () =>
        {
            handler = new AddClientAddressCommandHandler(addressDataManager, linkedKeyManager, domainRuleManager);
        };

        private It should_register_the_existing_active_client_address_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<AddressCannotBeAnExistingClientAddressRule>()));
        };
    }
}