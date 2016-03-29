using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Rules
{
    public class FixedPropertyAssetAcquiredDateCannotBeInTheFutureRule<T> : IDomainRule<T> where T : FixedPropertyAssetModel
    {
        public void ExecuteRule(ISystemMessageCollection messages, T ruleModel)
        {
            if (ruleModel.DateAquired > DateTime.Now)
            {
                messages.AddMessage(new SystemMessage("The acquisition date for a fixed property asset cannot be in the future.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}