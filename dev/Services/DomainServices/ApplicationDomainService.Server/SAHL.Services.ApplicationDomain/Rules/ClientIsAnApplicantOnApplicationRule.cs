using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class ClientIsAnApplicantOnApplicationRule : IDomainRule<ApplicantDeclarationsModel>
    {
        private IApplicantDataManager applicantDataManager;

        public ClientIsAnApplicantOnApplicationRule(IApplicantDataManager applicantDataManager)
        {
            this.applicantDataManager = applicantDataManager;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, ApplicantDeclarationsModel ruleModel)
        {
            var clientIsAnApplicantOnTheApplication = applicantDataManager.CheckClientIsAnApplicantOnTheApplication(ruleModel.ClientKey, ruleModel.ApplicationNumber);
            if (!clientIsAnApplicantOnTheApplication)
            {
                messages.AddMessage(new SystemMessage("The client is not an applicant on this application", SystemMessageSeverityEnum.Error));
            }
        }
    }
}
