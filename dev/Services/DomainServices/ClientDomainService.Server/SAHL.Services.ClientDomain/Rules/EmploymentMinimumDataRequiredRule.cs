using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Rules
{
    public class EmploymentMinimumDataRequiredRule<T> : IDomainRule<T> where T : EmploymentModel
    {
        private IEmploymentDataManager EmploymentDataManager;

        public EmploymentMinimumDataRequiredRule(IEmploymentDataManager employmentDataManager)
        {
            this.EmploymentDataManager = employmentDataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, T ruleModel)
        {
            if (EmploymentDataManager.FindExistingEmployer(ruleModel.Employer).Count() == 0)
            {
                messages.AddMessage(new SystemMessage("An Employer is required when adding an employment record for a client.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}