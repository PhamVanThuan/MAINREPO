using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.LifeDomain.CommandHandlers;
using SAHL.Services.LifeDomain.Managers;
using System;
using System.Linq;

namespace SAHL.Services.LifeDomain.Specs.CommandHandlersSpec.CapturePendingDisabilityClaim
{
    public class when_capturing_a_pending_disability_claim_with_rule_exception : WithCoreFakes
    {
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static IDomainRuleManager<IDisabilityClaimRuleModel> domainRuleManager;
        private static CapturePendingDisabilityClaimCommand command;
        private static CapturePendingDisabilityClaimCommandHandler handler;
        private static ISystemMessageCollection expectedMessages;
        private static string ruleFailureMessage;

        private Establish context = () =>
        {
            lifeDomainDataManager = An<ILifeDomainDataManager>();
            domainRuleManager = An<IDomainRuleManager<IDisabilityClaimRuleModel>>();
            eventRaiser = An<IEventRaiser>();
            expectedMessages = new SystemMessageCollection();
            ruleFailureMessage = "Rule Failure";
            expectedMessages.AddMessage(new SystemMessage(ruleFailureMessage, SystemMessageSeverityEnum.Error));
            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<IDisabilityClaimRuleModel>()))
                .Callback<ISystemMessageCollection, IDisabilityClaimRuleModel>((y, z) => { y.AddMessages(expectedMessages.AllMessages); });
            command = new CapturePendingDisabilityClaimCommand(1, DateTime.Now.AddDays(-5)
                , (int)DisabilityType.AlzheimersDisease, "Mental"
                , "Fireman", DateTime.Now.AddYears(-1)
                , DateTime.Now.AddYears(+1)
             );

            handler = new CapturePendingDisabilityClaimCommandHandler(lifeDomainDataManager, domainRuleManager, eventRaiser);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_not_update_the_disability_claim = () =>
        {
            lifeDomainDataManager.WasNotToldTo(x => x.UpdatePendingDisabilityClaim(Arg.Any<int>()
                , Arg.Any<DateTime>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<DateTime>(), Arg.Any<DateTime?>()));
        };

        private It should_return_the_rule_message = () =>
        {
            messages.ErrorMessages().Where(x => x.Message == ruleFailureMessage).First().ShouldNotBeNull();
        };

        private It should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<Event>(),
                    Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}