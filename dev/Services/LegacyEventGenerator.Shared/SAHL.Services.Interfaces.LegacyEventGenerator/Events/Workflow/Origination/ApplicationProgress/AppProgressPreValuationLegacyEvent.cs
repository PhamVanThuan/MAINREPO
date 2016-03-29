using System;

namespace SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.ApplicationProgress
{
    public class AppProgressPreValuationLegacyEvent : LegacyEvent
    {
        public AppProgressPreValuationLegacyEvent(Guid id, DateTime date, int adUserKey, string aduserName, int applicationKey,
            int applicationInformationKey, string assignedADUser, string commissionableADUser)
            : base(id, date, adUserKey, aduserName)
        {
            this.ApplicationKey = applicationKey;
            this.ApplicationInformationKey = applicationInformationKey;
            this.CommissionableADUser = commissionableADUser;
            this.AssignedADUser = assignedADUser;
        }

        public int ApplicationInformationKey { get; protected set; }

        public int ApplicationKey { get; protected set; }

        public string AssignedADUser { get; protected set; }

        public string CommissionableADUser { get; protected set; }
    }
}
