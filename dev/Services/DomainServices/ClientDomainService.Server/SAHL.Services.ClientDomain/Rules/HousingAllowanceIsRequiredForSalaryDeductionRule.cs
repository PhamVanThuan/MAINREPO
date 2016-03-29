using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Rules
{
    public class HousingAllowanceIsRequiredForSalaryDeductionRule : IDomainRule<SalaryDeductionEmploymentModel>
    {
        public void ExecuteRule(ISystemMessageCollection messages, SalaryDeductionEmploymentModel ruleModel)
        {
            if (ruleModel.HousingAllowance <= 0)
            {
                messages.AddMessage(new SystemMessage("Housing allowance is required and must be greater than R 0.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}