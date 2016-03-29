using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
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
    public class TerminateDisabilityClaimCommandHandler : IServiceCommandHandler<TerminateDisabilityClaimCommand>
    {
        private ILifeDomainDataManager lifeDomainDataManager;        
        private IEventRaiser eventRaiser;
        private IUnitOfWorkFactory uowFactory;

        public TerminateDisabilityClaimCommandHandler(ILifeDomainDataManager lifeDomainDataManager, IEventRaiser eventRaiser, IUnitOfWorkFactory uowFactory)
        {
            this.lifeDomainDataManager = lifeDomainDataManager;
            this.eventRaiser = eventRaiser;
            this.uowFactory = uowFactory;       
        }

        public ISystemMessageCollection HandleCommand(TerminateDisabilityClaimCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            using (var uow = uowFactory.Build())
            {                   
                lifeDomainDataManager.UpdateDisabilityClaimStatus(command.DisabilityClaimKey, DisabilityClaimStatus.Terminated);

                // call the halo api to terminate the payment schedule
                var terminateDisabilityClaimResult = lifeDomainDataManager.TerminateDisabilityClaimPaymentSchedule(command.DisabilityClaimKey, metadata.UserName);

                if (!string.IsNullOrEmpty(terminateDisabilityClaimResult))
                {
                    messages.AddMessage(new SystemMessage(terminateDisabilityClaimResult, SystemMessageSeverityEnum.Error));
                }
                else
                {
                    eventRaiser.RaiseEvent(DateTime.Now, new DisabilityClaimTerminatedEvent(DateTime.Now, command.DisabilityClaimKey, command.ReasonKey), 
                        command.DisabilityClaimKey, (int)GenericKeyType.DisabilityClaim, metadata);
                    uow.Complete();
                }
            }

            return messages;
        }
    }
}