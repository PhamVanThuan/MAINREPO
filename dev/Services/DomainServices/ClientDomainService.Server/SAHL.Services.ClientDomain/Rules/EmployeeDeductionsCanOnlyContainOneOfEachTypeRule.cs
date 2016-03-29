using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Rules
{
    public class EmployeeDeductionsCanOnlyContainOneOfEachTypeRule : IDomainRule<IEmployeeDeductions>
    {
        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, IEmployeeDeductions ruleModel)
        {
            if (ruleModel.EmployeeDeductions != null && ruleModel.EmployeeDeductions.Any())
            {
                var query = ruleModel.EmployeeDeductions.GroupBy(x => x.Type).Where(g => g.Count() > 1).Select(y => y);
                if (query.FirstOrDefault() != null)
                {
                    messages.AddMessage(new SystemMessage("The employee deductions for a client's employment should only contain one of each deduction type.",
                        SystemMessageSeverityEnum.Error));
                }
            }
        }
    }
}