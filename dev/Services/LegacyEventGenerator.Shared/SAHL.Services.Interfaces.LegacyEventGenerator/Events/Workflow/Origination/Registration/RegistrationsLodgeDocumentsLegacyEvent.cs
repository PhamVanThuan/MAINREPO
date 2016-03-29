using System;

namespace SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.Registration
{
    public class RegistrationsLodgeDocumentsLegacyEvent : LegacyEvent
    {
        public RegistrationsLodgeDocumentsLegacyEvent(Guid id, DateTime date, int adUserKey, string aduserName, int applicationKey,
            int applicationInformationKey, string assignedADUser, string commissionableAdUser)
            : base(id, date, adUserKey, aduserName)
        {
            this.ApplicationKey = applicationKey;
            this.ApplicationInformationKey = applicationInformationKey;
            this.CommissionableADUser = commissionableAdUser;
            this.AssignedADUser = assignedADUser;
        }

        public int ApplicationKey { get; protected set; }

        public int ApplicationInformationKey { get; protected set; }

        public string AssignedADUser { get; protected set; }

        public string CommissionableADUser { get; protected set; }
    }
}