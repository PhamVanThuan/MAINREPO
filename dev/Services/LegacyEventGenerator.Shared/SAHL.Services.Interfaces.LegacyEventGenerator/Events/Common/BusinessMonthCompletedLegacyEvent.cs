using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.LegacyEventGenerator.Events.Common
{
    public class BusinessMonthCompletedLegacyEvent : Event
    {
        public BusinessMonthCompletedLegacyEvent(DateTime date)
            : base(date)
        {
        }

        public BusinessMonthCompletedLegacyEvent(Guid id, DateTime date)
            : base(id, date)
        {
        }
    }
}