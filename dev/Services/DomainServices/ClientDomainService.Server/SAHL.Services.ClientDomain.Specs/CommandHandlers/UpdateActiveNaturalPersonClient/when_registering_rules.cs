using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.UpdateActiveNaturalPersonClient
{
    public class when_registering_rules : WithCoreFakes
    {
        public static UpdateActiveNaturalPersonClientCommandHandler handler;
        private static IClientDataManager clientDataManager;
        private static IDomainRuleManager<ActiveNaturalPersonClientModel> domainRuleManager;

        private Establish context = () =>
        {
            clientDataManager = An<IClientDataManager>();
            domainRuleManager = An<IDomainRuleManager<ActiveNaturalPersonClientModel>>();
        };

        private Because of = () =>
        {
            handler = new UpdateActiveNaturalPersonClientCommandHandler
                (clientDataManager, eventRaiser, domainRuleManager);
        };

        private It should_register_the_client_contact_details_rule = () =>
        {
            domainRuleManager.WasToldTo
                (x => x.RegisterPartialRule<IClientContactDetails>(Param.IsAny<AtLeastOneClientContactDetailShouldBeProvidedRule>()));
        };
    }
}