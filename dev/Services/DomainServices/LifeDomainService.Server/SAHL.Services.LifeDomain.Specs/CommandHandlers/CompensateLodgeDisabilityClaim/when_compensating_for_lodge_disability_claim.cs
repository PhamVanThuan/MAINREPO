using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.LifeDomain.CommandHandlers;
using SAHL.Services.LifeDomain.Managers;

namespace SAHL.Services.LifeDomain.Specs.CommandHandlersSpec.CompensateLodgeDisabilityClaim
{
    public class when_compensating_for_lodge_disability_claim : WithCoreFakes
    {
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static CompensateLodgeDisabilityClaimCommand command;
        private static CompensateLodgeDisabilityClaimCommandHandler handler;

        private static int disabilityClaimKey;

        private Establish context = () =>
        {
            disabilityClaimKey = 99999;

            lifeDomainDataManager = An<ILifeDomainDataManager>();

            command = new CompensateLodgeDisabilityClaimCommand(disabilityClaimKey);
            eventRaiser = An<IEventRaiser>();
            handler = new CompensateLodgeDisabilityClaimCommandHandler(lifeDomainDataManager, eventRaiser);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_not_return_any_messages = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };

        private It should_delete_the_disability_claim_record = () =>
        {
            lifeDomainDataManager.WasToldTo(x => x.DeleteDisabilityClaim(disabilityClaimKey));
        };
    }
}