using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class ClientBankAccountMustBelongToApplicantOnApplicationRule : IDomainRule<ApplicationDebitOrderModel>
    {
        private IApplicantDataManager applicantDataManager;

        public ClientBankAccountMustBelongToApplicantOnApplicationRule(IApplicantDataManager applicantDataManager)
        {
            this.applicantDataManager = applicantDataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, ApplicationDebitOrderModel ruleModel)
        {
            bool result = applicantDataManager.DoesBankAccountBelongToApplicantOnApplication(ruleModel.ApplicationNumber, ruleModel.ClientBankAccountKey);
            if(!result)
            {
                messages.AddMessage(new SystemMessage("The client bank account provided does not belong to an applicant on the application.",
                    SystemMessageSeverityEnum.Error));
            }
        }
    }
}
