using System;

namespace SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.BusinessApplicationProcess
{
    public class NewBusinessApplicationProgressDisbursedLegacyEvent :LegacyEvent
    {
        public NewBusinessApplicationProgressDisbursedLegacyEvent(Guid id, DateTime date, int adUserKey, string aduserName, 
            int applicationKey, int applicationInformationKey, string assignedAdUser, string commissionableAdUser) 
            : base(id, date, adUserKey, aduserName){
            CommissionableADUser = commissionableAdUser;
            AssignedADUser = assignedAdUser;
            ApplicationInformationKey = applicationInformationKey;
            ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }

        public int ApplicationInformationKey { get; protected set; }

        public string AssignedADUser { get; protected set; }

        public string CommissionableADUser { get; protected set; }

    }
}