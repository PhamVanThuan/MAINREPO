using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Identity;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Events;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.LifeDomain.CommandHandlers;
using SAHL.Services.LifeDomain.Managers;
using System;

namespace SAHL.Services.LifeDomain.Specs.CommandHandlersSpec.LodgeDisabilityClaim
{
    public class when_lodging_a_disability_claim : WithCoreFakes
    {
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static IDomainRuleManager<IDisabilityClaimLifeAccountModel> domainRuleManager;
        private static LodgeDisabilityClaimCommand command;
        private static LodgeDisabilityClaimCommandHandler handler;

        private static int lifeAccountKey;
        private static int claimantLegalEntityKey;
        private static int disabilityClaimKey;
        private static Guid disabilityClaimGuid;

        Establish context = () =>
        {
            lifeDomainDataManager = An<ILifeDomainDataManager>();
            domainRuleManager = An<IDomainRuleManager<IDisabilityClaimLifeAccountModel>>();
            eventRaiser = An<IEventRaiser>();
            lifeAccountKey = 3333333;
            claimantLegalEntityKey = 99999;
            disabilityClaimKey = 1;
            disabilityClaimGuid = CombGuid.Instance.Generate();
            command = new LodgeDisabilityClaimCommand(lifeAccountKey, claimantLegalEntityKey, disabilityClaimGuid);
            handler = new LodgeDisabilityClaimCommandHandler(lifeDomainDataManager, domainRuleManager, eventRaiser, linkedKeyManager);

            lifeDomainDataManager.WhenToldTo(x => x.LodgeDisabilityClaim(lifeAccountKey, claimantLegalEntityKey)).Return(disabilityClaimKey);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        It should_not_return_any_messages = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };

        It should_execute_the_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.ExecuteRules(Arg.Any<ISystemMessageCollection>(), command));
        };

        It should_lodge_the_disablity_claim = () =>
        {
            lifeDomainDataManager.WasToldTo(x => x.LodgeDisabilityClaim(lifeAccountKey, claimantLegalEntityKey));
        };

        It should_raise_an_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<DisabilityClaimLodgedEvent>(),
                    disabilityClaimKey, (int)GenericKeyType.DisabilityClaim, Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}