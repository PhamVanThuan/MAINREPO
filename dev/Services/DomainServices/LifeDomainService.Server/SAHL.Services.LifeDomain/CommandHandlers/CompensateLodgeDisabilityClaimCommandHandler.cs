using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Events;
using SAHL.Services.LifeDomain.Managers;
using System;

namespace SAHL.Services.LifeDomain.CommandHandlers
{
    public class CompensateLodgeDisabilityClaimCommandHandler : IServiceCommandHandler<CompensateLodgeDisabilityClaimCommand>
    {
        private ILifeDomainDataManager lifeDomainDataManager;
        private IEventRaiser eventRaiser;

        public CompensateLodgeDisabilityClaimCommandHandler(ILifeDomainDataManager lifeDomainDataManager, IEventRaiser eventRaiser)
        {
            this.lifeDomainDataManager = lifeDomainDataManager;
            this.eventRaiser = eventRaiser;
        }

        public ISystemMessageCollection HandleCommand(CompensateLodgeDisabilityClaimCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();
            lifeDomainDataManager.DeleteDisabilityClaim(command.DisabilityClaimKey);

            eventRaiser.RaiseEvent(DateTime.Now, new CompensateLodgeDisabilityClaimEvent(DateTime.Now, command.DisabilityClaimKey), command.DisabilityClaimKey, 
                (int)GenericKeyType.DisabilityClaim, metadata);

            return systemMessageCollection;
        }
    }
}