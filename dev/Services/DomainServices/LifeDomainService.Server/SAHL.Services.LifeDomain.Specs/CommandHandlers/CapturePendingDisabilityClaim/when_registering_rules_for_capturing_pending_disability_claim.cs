using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.LifeDomain.CommandHandlers;
using SAHL.Services.LifeDomain.Managers;
using SAHL.Services.LifeDomain.Rules;

namespace SAHL.Services.LifeDomain.Specs.CommandHandlersSpec.CapturePendingDisabilityClaim
{
    public class when_registering_rules_for_capturing_pending_disability_claim : WithCoreFakes
    {
        private static CapturePendingDisabilityClaimCommandHandler handler;
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static IDomainRuleManager<IDisabilityClaimRuleModel> domainRuleManager;

        private Establish context = () =>
            {
                lifeDomainDataManager = An<ILifeDomainDataManager>();
                domainRuleManager = An<IDomainRuleManager<IDisabilityClaimRuleModel>>();
            };

        private Because of = () =>
            {
                handler = new CapturePendingDisabilityClaimCommandHandler(lifeDomainDataManager, domainRuleManager, eventRaiser);
            };

        private It should_register_work_date_rule = () =>
            {
                domainRuleManager.RegisterPartialRule<IDisabilityClaimRuleModel>(Param.IsAny<ExpectedReturnDateMustBeAfterLastDateWorkedRule>());
            };
    }
}