using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Application;
using SAHL.DomainProcessManager.Models;
using System;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.Rules
{
    public class OpenMortgageLoanForApplicantAndComcorpPropertyCannotExistRule : IDomainRule<ApplicationCreationModel>
    {
        private IApplicationDataManager applicationDataManager;

        public OpenMortgageLoanForApplicantAndComcorpPropertyCannotExistRule(IApplicationDataManager applicationDataManager)
        {
            this.applicationDataManager = applicationDataManager;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, ApplicationCreationModel ruleModel)
        {
            foreach (var applicant in ruleModel.Applicants)
            {
                bool applicationExists = applicationDataManager.DoesOpenApplicationExistForComcorpProperty(applicant.IDNumber, ruleModel.ComcorpApplicationPropertyDetail);
                if (applicationExists)
                {
                    messages.AddMessage(new SystemMessage("An application for this property already exists against a client on this application.", SystemMessageSeverityEnum.Error));
                }
            }
        }
    }
}