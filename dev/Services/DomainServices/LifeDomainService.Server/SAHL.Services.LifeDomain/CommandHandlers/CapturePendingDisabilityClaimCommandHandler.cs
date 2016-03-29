using SAHL.Core.BusinessModel.Enums;
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.LifeDomain.CommandHandlers
{
    public class CapturePendingDisabilityClaimCommandHandler : IServiceCommandHandler<CapturePendingDisabilityClaimCommand>
    {
        private ILifeDomainDataManager lifeDomainDataManager;
        private IDomainRuleManager<IDisabilityClaimRuleModel> domainRuleManager;
        private IEventRaiser eventRaiser;

        public CapturePendingDisabilityClaimCommandHandler(ILifeDomainDataManager lifeDomainDataManager, IDomainRuleManager<IDisabilityClaimRuleModel> domainRuleManager, IEventRaiser eventRaiser)
        {
            this.lifeDomainDataManager = lifeDomainDataManager;
            this.domainRuleManager = domainRuleManager;
            this.eventRaiser = eventRaiser;
            this.domainRuleManager.RegisterPartialRule<IDisabilityClaimRuleModel>(new ExpectedReturnDateMustBeAfterLastDateWorkedRule(lifeDomainDataManager));
        }

        public ISystemMessageCollection HandleCommand(CapturePendingDisabilityClaimCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();
            domainRuleManager.ExecuteRules(systemMessageCollection, command);
            if (systemMessageCollection.HasErrors)
            {
                return systemMessageCollection;
            }
            lifeDomainDataManager.UpdatePendingDisabilityClaim(command.DisabilityClaimKey, command.DateOfDiagnosis, command.DisabilityTypeKey, command.OtherDisabilityComments,
                command.ClaimantOccupation, command.LastDateWorked, command.ExpectedReturnToWorkDate);

            eventRaiser.RaiseEvent(DateTime.Now, new DisabilityClaimCapturedEvent(DateTime.Now, command.DateOfDiagnosis, command.DisabilityTypeKey, command.OtherDisabilityComments,
                command.ClaimantOccupation, command.LastDateWorked, command.ExpectedReturnToWorkDate), command.DisabilityClaimKey, (int)GenericKeyType.DisabilityClaim, metadata);
            return systemMessageCollection;
        }
    }
}
