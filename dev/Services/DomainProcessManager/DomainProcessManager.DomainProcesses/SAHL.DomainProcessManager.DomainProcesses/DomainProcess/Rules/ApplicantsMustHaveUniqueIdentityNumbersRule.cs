using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.Rules
{
    public class ApplicantsMustHaveUniqueIdentityNumbersRule : IDomainRule<ApplicationCreationModel>
    {
        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, ApplicationCreationModel ruleModel)
        {
            if(ruleModel.Applicants != null && ruleModel.Applicants.Count() <= 1) { return; }
            
            List<String> duplicates = ruleModel.Applicants.GroupBy(x => x.IDNumber)
                            .Where(g => g.Count() > 1)
                            .Select(g => g.Key)
                            .ToList();
            if(duplicates.Any())
            {
                messages.AddMessage(new SystemMessage("The identity number for each applicant must be unique.", SystemMessageSeverityEnum.Error));        
            }
        }
    }
}