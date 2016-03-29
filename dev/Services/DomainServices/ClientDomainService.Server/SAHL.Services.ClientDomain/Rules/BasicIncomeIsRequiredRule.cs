using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Rules
{
    public class BasicIncomeIsRequiredRule<T> : IDomainRule<T> where T : EmploymentModel
    {
        public void ExecuteRule(ISystemMessageCollection messages, T ruleModel)
        {
            if (ruleModel.BasicIncome <= 0)
            {
                messages.AddMessage(new SystemMessage("Basic Income must be greater than zero", SystemMessageSeverityEnum.Error));
            }
        }
    }
}