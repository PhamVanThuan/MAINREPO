using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class ClientAddressMustBelongToAnApplicantRule : IDomainRule<ApplicationMailingAddressModel>
    {
        private IApplicantDataManager applicantDataManager;
        public ClientAddressMustBelongToAnApplicantRule(IApplicantDataManager applicantDataManager)
        {
            this.applicantDataManager = applicantDataManager;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, ApplicationMailingAddressModel ruleModel)
        {
            var clientAddressBelongsToApplicant = applicantDataManager.DoesClientAddressBelongToApplicant(ruleModel.ClientAddressKey, ruleModel.ApplicationNumber);
            if (!clientAddressBelongsToApplicant)
            {
                messages.AddMessage(new SystemMessage("The client address provided does not belong to an applicant.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}