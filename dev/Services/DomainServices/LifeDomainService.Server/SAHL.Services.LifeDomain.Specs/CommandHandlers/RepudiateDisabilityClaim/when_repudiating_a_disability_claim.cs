using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Events;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.LifeDomain.CommandHandlers;
using SAHL.Services.LifeDomain.Managers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SAHL.Services.LifeDomain.Specs.CommandHandlersSpec.RepudiateDisabilityClaim
{
    public class when_repudiating_a_disability_claim : WithCoreFakes
    {
        private static ILifeDomainDataManager lifeDomainDataManager;        
        private static RepudiateDisabilityClaimCommand command;
        private static RepudiateDisabilityClaimCommandHandler handler;
        private static int disabilityClaimKey;

        private Establish context = () =>
        {
            lifeDomainDataManager = An<ILifeDomainDataManager>();            
            eventRaiser = An<IEventRaiser>();
            disabilityClaimKey = 1;

            command = new RepudiateDisabilityClaimCommand(disabilityClaimKey, Param.IsAny<IEnumerable<int>>());
            handler = new RepudiateDisabilityClaimCommandHandler(lifeDomainDataManager, eventRaiser);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_update_the_status_of_the_disability_claim_to_repudiated = () =>
        {
            lifeDomainDataManager.WasToldTo(x => x.UpdateDisabilityClaimStatus(disabilityClaimKey, DisabilityClaimStatus.Repudiated));
        };

        private It should_raise_an_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<DisabilityClaimRepudiatedEvent>(),
                    disabilityClaimKey, (int)GenericKeyType.DisabilityClaim, Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_not_return_any_messages = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };
    }
}