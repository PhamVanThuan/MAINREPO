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

namespace SAHL.Services.LifeDomain.Specs.CommandHandlersSpec.TerminateDisabilityClaim
{
    public class when_terminating_a_disability_claim_with_api_exception : WithCoreFakes
    {
        private static ILifeDomainDataManager lifeDomainDataManager;        
        private static TerminateDisabilityClaimCommand command;
        private static TerminateDisabilityClaimCommandHandler handler;
        private static int disabilityClaimKey;

        private Establish context = () =>
        {
            lifeDomainDataManager = An<ILifeDomainDataManager>();            
            eventRaiser = An<IEventRaiser>();
            disabilityClaimKey = 1;

            command = new TerminateDisabilityClaimCommand(disabilityClaimKey, Param.IsAny<int>());
            handler = new TerminateDisabilityClaimCommandHandler(lifeDomainDataManager, eventRaiser, unitOfWorkFactory);

            lifeDomainDataManager.WhenToldTo(x => x.TerminateDisabilityClaimPaymentSchedule(disabilityClaimKey, "")).Return("API Call Failed");
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_update_the_status_of_the_disability_claim_to_terminated = () =>
        {
            lifeDomainDataManager.WasToldTo(x => x.UpdateDisabilityClaimStatus(disabilityClaimKey, DisabilityClaimStatus.Terminated));
        };

        private It should_call_the_halo_api_to_terminate_the_payment_schedule = () =>
        {
            lifeDomainDataManager.WasToldTo(x => x.TerminateDisabilityClaimPaymentSchedule(disabilityClaimKey, Param.IsAny<string>()));
        };

        private It should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<DisabilityClaimTerminatedEvent>(),
                    disabilityClaimKey, (int)GenericKeyType.DisabilityClaim, Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_return_error_messages = () =>
        {
            messages.AllMessages.ShouldNotBeEmpty();
        };
    }
}