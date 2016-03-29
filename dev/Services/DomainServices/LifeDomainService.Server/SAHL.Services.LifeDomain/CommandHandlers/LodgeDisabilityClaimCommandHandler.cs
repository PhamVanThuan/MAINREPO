using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Events;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.LifeDomain.Managers;
using SAHL.Services.LifeDomain.Rules;
using System;

namespace SAHL.Services.LifeDomain.CommandHandlers
{
    public class LodgeDisabilityClaimCommandHandler : IServiceCommandHandler<LodgeDisabilityClaimCommand>
    {
        private ILifeDomainDataManager lifeDomainDataManager;
        private IDomainRuleManager<IDisabilityClaimLifeAccountModel> domainRuleManager;
        private IEventRaiser eventRaiser;
        private ILinkedKeyManager linkedKeyManager;

        public LodgeDisabilityClaimCommandHandler(ILifeDomainDataManager lifeDomainDataManager, IDomainRuleManager<IDisabilityClaimLifeAccountModel> domainRuleManager, 
            IEventRaiser eventRaiser, ILinkedKeyManager linkedKeyManager)
        {
            this.lifeDomainDataManager = lifeDomainDataManager;
            this.domainRuleManager = domainRuleManager;
            this.eventRaiser = eventRaiser;
            this.linkedKeyManager = linkedKeyManager;

            this.domainRuleManager.RegisterPartialRule <IDisabilityClaimLifeAccountModel>(new AccountCanOnlyHaveOnePendingOrApprovedDisabilityClaimRule(lifeDomainDataManager));
        }

        public ISystemMessageCollection HandleCommand(LodgeDisabilityClaimCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();
             
            domainRuleManager.ExecuteRules(systemMessageCollection, command);
            
            if (systemMessageCollection.HasErrors)
            {
                return systemMessageCollection;
            }
            
            var disablityClaimKey = lifeDomainDataManager.LodgeDisabilityClaim(command.LifeAccountKey, command.ClaimantLegalEntityKey);

            this.linkedKeyManager.LinkKeyToGuid(disablityClaimKey, command.DisabilityClaimGuid);

            eventRaiser.RaiseEvent(DateTime.Now, new DisabilityClaimLodgedEvent(DateTime.Now, command.LifeAccountKey, command.ClaimantLegalEntityKey), disablityClaimKey, 
                (int)GenericKeyType.DisabilityClaim, metadata);
            
            return systemMessageCollection;
        }
    }
}