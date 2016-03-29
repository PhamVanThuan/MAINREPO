using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.ClientDomain.Rules
{
    public class ClientDateOfBirthMustBePriorToTodayRule : IDomainRule<INaturalPersonClientModel>
    {
        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, INaturalPersonClientModel ruleModel)
        {
            if (ruleModel.DateOfBirth != null && ruleModel.DateOfBirth > DateTime.Today)
            {
                messages.AddMessage(new SystemMessage("Client date of birth is invalid.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}