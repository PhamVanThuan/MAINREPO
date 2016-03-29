using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.ApplicationProgress
{
    /**************************************************************
    * CompositeKey : 1460 & 4416
    */

    public class AppProgressNTUPendingLegacyEvent : IEvent
    {
        public AppProgressNTUPendingLegacyEvent(Guid id, DateTime eventDate, int applicationKey, int applicationInformationKey, string consultantDueCommission, string assignedADUser)
        {
            this.Id = id;
            this.Date = eventDate;
            this.ApplicationKey = applicationKey;
            this.ConsultantDueCommission = consultantDueCommission;
            this.AssignedADUser = assignedADUser;
        }

        public int EventType
        {
            get { return -1; }
        }

        public string Name
        {
            get { return "AppProgressNTUPending"; }
        }

        public Guid Id
        {
            get;
            protected set;
        }

        public DateTime Date
        {
            get;
            protected set;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }

        public int ApplicationInformationKey
        {
            get;
            protected set;
        }

        public string ConsultantDueCommission { get; protected set; }

        public string AssignedADUser { get; protected set; }

        public AppProgressEnum PreviousAppProgress { get; protected set; }
    }
}