using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.LegacyEventGenerator.Events
{
    public class LegacyEvent : Event, ILegacyEvent
    {
        public LegacyEvent(Guid id, DateTime date, int adUserKey, string aduserName)
           : base(id, date)
        {
            this.AdUserKey = adUserKey;
            this.AdUserName = aduserName;
        }

        public int AdUserKey { get; protected set; }

        public string AdUserName { get; protected set; }
    }
}