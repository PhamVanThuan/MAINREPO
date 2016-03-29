using System;

namespace SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Credit
{
    /**************************************************************
    * StageDefinitionStageDefinitionGroupKey : 1398
    */

    public class QACompletedLegacyEvent : LegacyEvent
    {
        public QACompletedLegacyEvent(Guid id, DateTime date, int adUserKey, string aduserName, int applicationKey)
           : base(id, date, adUserKey, aduserName)
        {
            this.ApplicationKey = applicationKey;
        }

        public new string Name { get { return "QACompleted"; } }

        public int ApplicationKey { get; protected set; }
    }
}