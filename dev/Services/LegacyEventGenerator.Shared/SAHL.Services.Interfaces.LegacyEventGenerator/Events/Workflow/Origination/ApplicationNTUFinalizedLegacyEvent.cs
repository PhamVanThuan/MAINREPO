using System;

namespace SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination
{
    /**************************************************************
    * StageDefinitionStageDefinitionGroupKey : 110
    */

    public class ApplicationNTUFinalizedLegacyEvent : LegacyEvent
    {
        public ApplicationNTUFinalizedLegacyEvent(Guid id, DateTime date, int adUserKey, string aduserName, int applicationKey, 
            int applicationInformationKey, string declineReason, string performedByADUser) : base(id, date, adUserKey, aduserName)
        {
            this.ApplicationKey = applicationKey;
            this.ApplicationInformationKey = applicationInformationKey;
            this.DeclineReason = declineReason;
            this.PerformedByADUser = performedByADUser;
        }

        public int ApplicationKey { get; protected set; }

        public int ApplicationInformationKey { get; protected set; }

        public string DeclineReason { get; protected set; }

        public string PerformedByADUser { get; protected set; }
    }
}