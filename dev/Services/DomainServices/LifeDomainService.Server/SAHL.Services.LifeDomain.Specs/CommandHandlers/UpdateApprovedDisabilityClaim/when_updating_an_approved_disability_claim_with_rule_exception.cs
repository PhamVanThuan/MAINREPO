using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
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
using System.Collections;
using System.Collections.Generic;

namespace SAHL.Services.LifeDomain.Specs.CommandHandlersSpec.UpdateApprovedDisabilityClaim
{
    public class when_updating_an_approved_disability_claim_with_rule_exception : WithCoreFakes
    {
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static IDomainRuleManager<IDisabilityClaimRuleModel> domainRuleManager;
        private static AmendApprovedDisabilityClaimCommand command;
        private static AmendApprovedDisabilityClaimCommandHandler handler;
        private static int disabilityClaimKey;

        private Establish context = () =>
        {
            lifeDomainDataManager = An<ILifeDomainDataManager>();
            domainRuleManager = An<IDomainRuleManager<IDisabilityClaimRuleModel>>();
            eventRaiser = An<IEventRaiser>();
            disabilityClaimKey = 1;


            command = new AmendApprovedDisabilityClaimCommand(
                               disabilityClaimKey
                             , Param.IsAny<int>()
                             , Param.IsAny<string>()
                             , Param.IsAny<string>()
                             , Param.IsAny<DateTime>()
                             , Param.IsAny<DateTime?>()
                       );

            handler = new AmendApprovedDisabilityClaimCommandHandler(lifeDomainDataManager, domainRuleManager, eventRaiser);

            var expectedMessages = SystemMessageCollection.Empty();
            expectedMessages.AddMessage(new SystemMessage("error message", SystemMessageSeverityEnum.Error));

            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<IDisabilityClaimRuleModel>()))
            .Callback<ISystemMessageCollection, IDisabilityClaimRuleModel>((y, z) => { y.AddMessages(expectedMessages.AllMessages); });
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_not_update_the_approved_disability_claim = () =>
        {
            lifeDomainDataManager.WasNotToldTo(x => x.UpdateApprovedDisabilityClaim(disabilityClaimKey
                                                       , Param.IsAny<int>(), Param.IsAny<string>()
                                                       , Param.IsAny<string>(), Param.IsAny<DateTime?>()
                                                    ));
        };

        private It should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<DisabilityClaimApproveAmendedEvent>(),
                    disabilityClaimKey, (int)GenericKeyType.DisabilityClaim, Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_return_error_messages = () =>
        {
            messages.AllMessages.ShouldNotBeEmpty();
        };
    }
}