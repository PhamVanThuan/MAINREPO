using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Domicilium;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class ApplicantCanOnlyBeLinkedToAPendingDomiciliumAddressRule : IDomainRule<ApplicantDomiciliumAddressModel>
    {
        private IDomiciliumDataManager domiciliumDataManager;

        public ApplicantCanOnlyBeLinkedToAPendingDomiciliumAddressRule(IDomiciliumDataManager domiciliumDataManager)
        {
            this.domiciliumDataManager = domiciliumDataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, ApplicantDomiciliumAddressModel ruleModel)
        {
            bool domiciliumAddressIsPendingAddress = domiciliumDataManager.IsDomiciliumAddressPendingDomiciliumAddress(ruleModel.ClientDomiciliumKey);
            if (!domiciliumAddressIsPendingAddress)
            {
                messages.AddMessage(new SystemMessage("An applicant can only be linked to a domicilium address that is marked as pending.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}