using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.LifeDomain.Managers;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.LifeDomain.Rules
{
    public class AccountCanOnlyHaveOnePendingOrApprovedDisabilityClaimRule : IDomainRule<IDisabilityClaimLifeAccountModel>
    {
        private ILifeDomainDataManager lifeDomainDataManager;

        public AccountCanOnlyHaveOnePendingOrApprovedDisabilityClaimRule(ILifeDomainDataManager lifeDomainDataManager)
        {
            this.lifeDomainDataManager = lifeDomainDataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, IDisabilityClaimLifeAccountModel ruleModel)
        {
            IEnumerable<DisabilityClaimModel> disabilityClaims = lifeDomainDataManager.GetDisabilityClaimsByAccount(ruleModel.LifeAccountKey);
            if (disabilityClaims.Where(x => (x.DisabilityClaimStatusKey == (int)DisabilityClaimStatus.Pending || x.DisabilityClaimStatusKey == (int)DisabilityClaimStatus.Approved)).Any())
            {
                messages.AddMessage(new SystemMessage("The client already has a pending or approved disability claim.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}