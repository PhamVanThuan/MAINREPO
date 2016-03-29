using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Events;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.LifeDomain.Managers;
using SAHL.Services.LifeDomain.Rules;
using System;

namespace SAHL.Services.LifeDomain.CommandHandlers
{
    public class AmendApprovedDisabilityClaimCommandHandler : IServiceCommandHandler<AmendApprovedDisabilityClaimCommand>
    {
        private ILifeDomainDataManager lifeDomainDataManager;
        private IDomainRuleManager<IDisabilityClaimRuleModel> domainRuleManager;
        private IEventRaiser eventRaiser;

        public AmendApprovedDisabilityClaimCommandHandler(ILifeDomainDataManager lifeDomainDataManager, IDomainRuleManager<IDisabilityClaimRuleModel> domainRuleManager, IEventRaiser eventRaiser)
        {
            this.lifeDomainDataManager = lifeDomainDataManager;
            this.domainRuleManager = domainRuleManager;
            this.eventRaiser = eventRaiser;

            this.domainRuleManager.RegisterPartialRule<IDisabilityClaimRuleModel>(new ExpectedReturnDateMustBeAfterLastDateWorkedRule(lifeDomainDataManager));
        }

        public ISystemMessageCollection HandleCommand(AmendApprovedDisabilityClaimCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            domainRuleManager.ExecuteRules(messages, command);
            if (messages.HasErrors)
            {
                return messages;
            }

            lifeDomainDataManager.UpdateApprovedDisabilityClaim(command.DisabilityClaimKey, command.DisabilityTypeKey, command.OtherDisabilityComments, 
                command.ClaimantOccupation, command.ExpectedReturnToWorkDate);

            eventRaiser.RaiseEvent(DateTime.Now, new DisabilityClaimApproveAmendedEvent(DateTime.Now, command.DisabilityTypeKey, command.OtherDisabilityComments, 
                command.ClaimantOccupation, command.ExpectedReturnToWorkDate), command.DisabilityClaimKey, (int)GenericKeyType.DisabilityClaim, metadata);

            return messages;
        }
    }
}