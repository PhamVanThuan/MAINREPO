using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.AddressDomain.CommandHandlers.Internal;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.AddressDomain.Rules;

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.AddStreetAddress
{
    public class when_registering_rules : WithCoreFakes
    {
        private static AddStreetAddressCommandHandler handler;
        private static IDomainRuleManager<AddressDataModel> domainRuleManager;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<AddressDataModel>>();
        };

        private Because of = () =>
        {
            handler = new AddStreetAddressCommandHandler(domainRuleManager, An<IAddressDataManager>(), An<ILinkedKeyManager>());
        };

        private It should_register_the_postal_address_validation_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<StreetAddressRequiresAValidSuburbRule>()));
        };
    }
}