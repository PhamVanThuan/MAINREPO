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
using SAHL.Services.Interfaces.LifeDomain.Events;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.LifeDomain.CommandHandlers;
using SAHL.Services.LifeDomain.Managers;
using System;

namespace SAHL.Services.LifeDomain.Specs.CommandHandlersSpec.CapturePendingDisabilityClaim
{
    public class when_capturing_a_pending_disability_claim : WithCoreFakes
    {
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static IDomainRuleManager<IDisabilityClaimRuleModel> domainRuleManager;
        private static CapturePendingDisabilityClaimCommand command;
        private static CapturePendingDisabilityClaimCommandHandler handler;

        private Establish context = () =>
        {
            lifeDomainDataManager = An<ILifeDomainDataManager>();
            domainRuleManager = An<IDomainRuleManager<IDisabilityClaimRuleModel>>();
            eventRaiser = An<IEventRaiser>();
            command = new CapturePendingDisabilityClaimCommand(1, DateTime.Now.AddDays(-5), (int)DisabilityType.AlzheimersDisease
                , "Mental", "Fireman", DateTime.Now.AddYears(-1), DateTime.Now.AddYears(+1));
            handler = new CapturePendingDisabilityClaimCommandHandler(lifeDomainDataManager, domainRuleManager, eventRaiser);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_not_return_any_messages = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };

        private It should_execute_the_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.ExecuteRules(Arg.Any<ISystemMessageCollection>(), command));
        };

        private It should_update_the_disability_claim_record = () =>
        {
            lifeDomainDataManager.WasToldTo(x => x.UpdatePendingDisabilityClaim(command.DisabilityClaimKey, command.DateOfDiagnosis, command.DisabilityTypeKey,
                command.OtherDisabilityComments, command.ClaimantOccupation, command.LastDateWorked, command.ExpectedReturnToWorkDate));
        };

        private It should_raise_an_event_with_the_properties_from_the_command = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<DisabilityClaimCapturedEvent>(
                y=> y.ClaimantOccupation == command.ClaimantOccupation && y.DateOfDiagnosis == command.DateOfDiagnosis &&
                    y.DisabilityTypeKey == command.DisabilityTypeKey && y.ExpectedReturnToWorkDate == command.ExpectedReturnToWorkDate &&
                    y.OtherDisabilityComments == command.OtherDisabilityComments && y.LastDateWorked == command.LastDateWorked
                ), command.DisabilityClaimKey, (int)GenericKeyType.DisabilityClaim, serviceRequestMetaData));
        };
    }
}