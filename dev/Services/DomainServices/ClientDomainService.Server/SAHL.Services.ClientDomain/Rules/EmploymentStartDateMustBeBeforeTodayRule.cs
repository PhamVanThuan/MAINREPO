using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Rules
{
    public class EmploymentStartDateMustBeBeforeTodayRule<T> : IDomainRule<T> where T : EmploymentModel
    {
        public void ExecuteRule(ISystemMessageCollection messages, T ruleModel)
        {
            if (ruleModel.StartDate > DateTime.Today)
            {
                messages.AddMessage(new SystemMessage("The start date of a current employment record cannot be in the future.", SystemMessageSeverityEnum.Error));
            }
            else if (ruleModel.StartDate == DateTime.Today)
            {
                messages.AddMessage(new SystemMessage("The start date of a current employment record cannot be today.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}