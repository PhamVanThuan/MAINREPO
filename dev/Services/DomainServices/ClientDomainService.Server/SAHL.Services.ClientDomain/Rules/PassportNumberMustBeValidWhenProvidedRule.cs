using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.ClientDomain.Rules
{
    public class PassportNumberMustBeValidWhenProvidedRule : IDomainRule<INaturalPersonClientModel>
    {
        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, INaturalPersonClientModel model)
        {
            if (!String.IsNullOrWhiteSpace(model.PassportNumber))
            {
                if (model.PassportNumber.Length < 6)
                {
                    messages.AddMessage(new SystemMessage("The passport number must be more than 6 digits.", SystemMessageSeverityEnum.Error));
                }
            }
        }
    }
}