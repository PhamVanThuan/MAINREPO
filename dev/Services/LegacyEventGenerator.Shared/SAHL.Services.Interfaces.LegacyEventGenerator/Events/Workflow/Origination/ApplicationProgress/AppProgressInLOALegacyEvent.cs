using System;

namespace SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.ApplicationProgress
{
    /**************************************************************
    * StageDefinitionStageDefinitionGroupKey : 72
    */

    public class AppProgressInLOALegacyEvent : LegacyEvent
    {
        public AppProgressInLOALegacyEvent(Guid id, DateTime date, int adUserKey, string aduserName, int applicationKey, 
            int applicationInformationKey, string assignedADUser, string commissionableADUser, AppProgressEnum previousAppProgress)
            : base(id, date, adUserKey, aduserName)
        {
            this.ApplicationKey = applicationKey;
            this.ApplicationInformationKey = applicationInformationKey;
            this.CommissionableADUser = commissionableADUser;
            this.AssignedADUser = assignedADUser;
            this.PreviousAppProgress = previousAppProgress;
        }

        public int ApplicationKey { get; protected set; }

        public int ApplicationInformationKey { get; protected set; }

        public string AssignedADUser { get; protected set; }

        public string CommissionableADUser { get; protected set; }

        public AppProgressEnum PreviousAppProgress { get; protected set; }
    }
}