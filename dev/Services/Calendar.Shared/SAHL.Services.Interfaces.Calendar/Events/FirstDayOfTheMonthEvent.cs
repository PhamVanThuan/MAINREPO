using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.Calendar.Events
{
    public class FirstDayOfTheMonthEvent : Event
    {
        public FirstDayOfTheMonthEvent(DateTime date)
            : base(date)
        {
        }
    }
}
