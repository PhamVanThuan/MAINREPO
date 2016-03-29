using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.FinancialDomain.Events
{
    public class NewBusinessApplicationFundedEvent : Event
    {
        public int ApplicationNumber { get; protected set; }

        public NewBusinessApplicationFundedEvent(DateTime date, int ApplicationNumber)
            : base(date)
        {
            this.ApplicationNumber = ApplicationNumber;
        }
    }
}
