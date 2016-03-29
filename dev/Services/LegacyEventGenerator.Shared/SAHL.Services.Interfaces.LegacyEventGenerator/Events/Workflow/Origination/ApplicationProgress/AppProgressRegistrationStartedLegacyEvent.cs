using System;

namespace SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.ApplicationProgress
{
    /**************************************************************
    * StageDefinitionStageDefinitionGroupKey : 74
    */

    public class AppProgressRegistrationStartedLegacyEvent : LegacyEvent
    {
        public AppProgressRegistrationStartedLegacyEvent(Guid id, DateTime date, int adUserKey, string aduserName, int applicationKey, 
            int applicationInformationKey, string assignedADUser, string commissionableADUser) : base(id, date, adUserKey, aduserName)
        {
            this.ApplicationKey = applicationKey;
            this.ApplicationInformationKey = applicationInformationKey;
            this.AssignedADUser = assignedADUser;
            this.CommissionableADUser = commissionableADUser;
        }

        public int ApplicationKey { get; protected set; }

        public int ApplicationInformationKey { get; protected set; }

        public string AssignedADUser { get; protected set; }

        public string CommissionableADUser { get; protected set; }
    }
}