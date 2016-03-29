using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.LifeDomain.Managers;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.LifeDomain.Rules
{
    public class ExpectedReturnDateMustBeAfterLastDateWorkedRule : IDomainRule<IDisabilityClaimRuleModel>
    {
        private ILifeDomainDataManager lifeDomainDataManager;

        public ExpectedReturnDateMustBeAfterLastDateWorkedRule(ILifeDomainDataManager lifeDomainDataManager)
        {
            this.lifeDomainDataManager = lifeDomainDataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, IDisabilityClaimRuleModel ruleModel)
        {
            if (ruleModel.ExpectedReturnToWorkDate < ruleModel.LastDateWorked)
            {
                messages.AddMessage(new SystemMessage("Expected Return to Work Date cannot be before Claimant's Last Date Worked.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}