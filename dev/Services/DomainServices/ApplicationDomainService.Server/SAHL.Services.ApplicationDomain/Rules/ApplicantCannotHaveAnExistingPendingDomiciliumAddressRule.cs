using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class ApplicantCannotHaveAnExistingPendingDomiciliumAddressRule : IDomainRule<ApplicantDomiciliumAddressModel>
    {
        private IApplicantDataManager applicantDataManager;

        public ApplicantCannotHaveAnExistingPendingDomiciliumAddressRule(IApplicantDataManager applicantDataManager)
        {
            this.applicantDataManager = applicantDataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, ApplicantDomiciliumAddressModel ruleModel)
        {
            bool applicantHasPendingDomicilium = applicantDataManager.DoesApplicantHavePendingDomiciliumOnApplication(ruleModel.ClientKey, ruleModel.ApplicationNumber);
            if (applicantHasPendingDomicilium)
            {
                messages.AddMessage(new SystemMessage("The applicant currently has a pending domicilium address.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}
