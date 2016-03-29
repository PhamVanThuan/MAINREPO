using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.ApplicationDomain.Events
{
    public class PropertyLinkedToApplicationEvent : Event
    {
        public int ApplicationNumber { get; protected set; }
        public int PropertyKey { get; protected set; }

        public PropertyLinkedToApplicationEvent(int ApplicationNumber, int PropertyKey, DateTime date)
            : base(date)
        {
            this.ApplicationNumber = ApplicationNumber;
            this.PropertyKey = PropertyKey;
        }
    }
}
