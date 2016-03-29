using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Events;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using SAHL.Services.LifeDomain.Managers;
using SAHL.Services.LifeDomain.Rules;
using System;

namespace SAHL.Services.LifeDomain.CommandHandlers
{
    public class RepudiateDisabilityClaimCommandHandler : IServiceCommandHandler<RepudiateDisabilityClaimCommand>
    {
        private ILifeDomainDataManager lifeDomainDataManager;        
        private IEventRaiser eventRaiser;

        public RepudiateDisabilityClaimCommandHandler(ILifeDomainDataManager lifeDomainDataManager, IEventRaiser eventRaiser)
        {
            this.lifeDomainDataManager = lifeDomainDataManager;            
            this.eventRaiser = eventRaiser;            
        }

        public ISystemMessageCollection HandleCommand(RepudiateDisabilityClaimCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            lifeDomainDataManager.UpdateDisabilityClaimStatus(command.DisabilityClaimKey, DisabilityClaimStatus.Repudiated);

            eventRaiser.RaiseEvent(DateTime.Now, new DisabilityClaimRepudiatedEvent(DateTime.Now, command.DisabilityClaimKey, command.ReasonKeys), 
                command.DisabilityClaimKey, (int)GenericKeyType.DisabilityClaim, metadata);

            return messages;
        }
    }
}