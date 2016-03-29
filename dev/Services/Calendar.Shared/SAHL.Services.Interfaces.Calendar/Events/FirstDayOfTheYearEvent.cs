using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.Calendar.Events
{
    public class FirstDayOfTheYearEvent : Event
    {
        public FirstDayOfTheYearEvent(DateTime date)
            : base(date)
        {
        }
    }
}
