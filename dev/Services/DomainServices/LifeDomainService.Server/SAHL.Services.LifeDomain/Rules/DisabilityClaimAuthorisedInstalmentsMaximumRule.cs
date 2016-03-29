using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.LifeDomain.Managers;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.LifeDomain.Rules
{
    public class DisabilityClaimAuthorisedInstalmentsMaximumRule : IDomainRule<IDisabilityClaimApproveModel>
    {
        private ILifeDomainDataManager lifeDomainDataManager;

        public DisabilityClaimAuthorisedInstalmentsMaximumRule(ILifeDomainDataManager lifeDomainDataManager)
        {
            this.lifeDomainDataManager = lifeDomainDataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, IDisabilityClaimApproveModel ruleModel)
        {
            if (ruleModel.NumberOfInstalmentsAuthorised > 99)
            {
                messages.AddMessage(new SystemMessage("No. of Authorised Instalments cannot be greater than 99.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}