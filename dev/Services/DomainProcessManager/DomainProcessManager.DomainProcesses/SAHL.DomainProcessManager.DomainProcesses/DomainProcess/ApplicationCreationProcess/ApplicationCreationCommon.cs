using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.DomainProcess;
using SAHL.Core.Rules;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.Rules;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Application;
using SAHL.DomainProcessManager.Models;
using System;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T> where T : ApplicationCreationModel
    {
        public bool ValidateApplication(IApplicationStateMachine stateMachine, IDomainRuleManager<ApplicationCreationModel> domainRuleManager)
        {
            bool isValid = true;
            domainRuleManager.RegisterRuleForContext(new ApplicationMustHaveAtLeastOneMainApplicantRule(), OriginationSource.Comcorp);
            domainRuleManager.RegisterRuleForContext(new ApplicationMustHaveAtLeastOneMainApplicantRule(), OriginationSource.SAHomeLoans);
            domainRuleManager.RegisterRuleForContext(new OpenMortgageLoanForApplicantAndComcorpPropertyCannotExistRule(this.applicationDataManager), OriginationSource.Comcorp);
            domainRuleManager.RegisterRuleForContext(new VendorMustExistForVendorCodeRule(this.applicationDataManager), OriginationSource.Comcorp);
            domainRuleManager.RegisterRuleForContext(new OpenMortgageLoanForApplicantAndPropertyCannotExistRule(this.PropertyDomainService, this.applicationDomainService), 
                OriginationSource.SAHomeLoans);
            domainRuleManager.RegisterRuleForContext(new OpenMortgageLoanForApplicantAndPropertyCannotExistRule(this.PropertyDomainService, this.applicationDomainService), 
                OriginationSource.Comcorp);
            domainRuleManager.RegisterRuleForContext(new ApplicantsMustHaveUniqueIdentityNumbersRule(), OriginationSource.Comcorp);
            domainRuleManager.RegisterRuleForContext(new ApplicantsMustHaveUniqueIdentityNumbersRule(), OriginationSource.SAHomeLoans);
            domainRuleManager.ExecuteRulesForContext(stateMachine.SystemMessages, this.DataModel, this.DataModel.OriginationSource);

            if (stateMachine.SystemMessages.HasErrors)
            {
                isValid = false;
                stateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.CriticalErrorReported, Guid.NewGuid());
            }
            return isValid;
        }
    }
}