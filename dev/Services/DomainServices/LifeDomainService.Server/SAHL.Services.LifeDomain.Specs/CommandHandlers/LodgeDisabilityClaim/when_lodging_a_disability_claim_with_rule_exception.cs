using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Identity;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.LifeDomain.CommandHandlers;
using SAHL.Services.LifeDomain.Managers;
using System;

namespace SAHL.Services.LifeDomain.Specs.CommandHandlersSpec.LodgeDisabilityClaim
{
    public class when_lodging_a_disability_claim_with_rule_exception : WithCoreFakes
    {
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static IDomainRuleManager<IDisabilityClaimLifeAccountModel> domainRuleManager;
        private static LodgeDisabilityClaimCommand command;
        private static LodgeDisabilityClaimCommandHandler handler;
        private static ISystemMessageCollection expectedMessages;
        private static Guid disabilityClaimGuid;

        private static int lifeAccountKey;
        private static int claimantLegalEntityKey;

        private Establish context = () =>
        {
            lifeDomainDataManager = An<ILifeDomainDataManager>();
            domainRuleManager = An<IDomainRuleManager<IDisabilityClaimLifeAccountModel>>();
            eventRaiser = An<IEventRaiser>();

            lifeAccountKey = 3333333;
            claimantLegalEntityKey = 99999;
            disabilityClaimGuid = CombGuid.Instance.Generate();

            expectedMessages = new SystemMessageCollection();
            expectedMessages.AddMessage(new SystemMessage("error message", SystemMessageSeverityEnum.Error));
            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<IDisabilityClaimLifeAccountModel>()))
                             .Callback<ISystemMessageCollection, IDisabilityClaimLifeAccountModel>((y, z) => { y.AddMessages(expectedMessages.AllMessages); });

            command = new LodgeDisabilityClaimCommand(lifeAccountKey, claimantLegalEntityKey, disabilityClaimGuid);
            handler = new LodgeDisabilityClaimCommandHandler(lifeDomainDataManager, domainRuleManager, eventRaiser, linkedKeyManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_register_client_can_only_have_one_pending_disability_claim_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterPartialRule(Param.IsAny<IDomainRule<IDisabilityClaimLifeAccountModel>>()));
        };

        private It should_run_the_client_can_only_have_one_pending_disability_claim_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<IDisabilityClaimLifeAccountModel>()));
        };

        private It should_not_insert_the_disability_claim = () =>
        {
            lifeDomainDataManager.WasNotToldTo(x => x.LodgeDisabilityClaim(lifeAccountKey,claimantLegalEntityKey));
        };

        private It should_return_errors = () =>
        {
            messages.ErrorMessages().ShouldNotBeEmpty();
        };

        private It should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<Event>(),
                    Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}