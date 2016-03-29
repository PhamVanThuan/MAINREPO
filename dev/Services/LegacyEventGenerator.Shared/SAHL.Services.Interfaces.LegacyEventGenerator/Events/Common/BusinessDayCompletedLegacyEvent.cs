using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.LegacyEventGenerator.Events.Common
{
    public class BusinessDayCompletedLegacyEvent : Event
    {
        public BusinessDayCompletedLegacyEvent(DateTime date)
            : base(date)
        {
        }

        public BusinessDayCompletedLegacyEvent(Guid id, DateTime date)
            : base(id, date)
        {
        }
    }
}