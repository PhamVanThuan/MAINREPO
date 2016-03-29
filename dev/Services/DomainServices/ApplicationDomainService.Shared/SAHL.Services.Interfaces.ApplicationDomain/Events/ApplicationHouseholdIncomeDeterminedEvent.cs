using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.ApplicationDomain.Events
{
    public class ApplicationHouseholdIncomeDeterminedEvent : Event
    {
        public ApplicationHouseholdIncomeDeterminedEvent(DateTime date, int applicationNumber, double householdIncome)
            : base(date)
        {
            this.ApplicationNumber = applicationNumber;
            this.HouseholdIncome = householdIncome;
        }

        public int ApplicationNumber { get; protected set; }

        public double HouseholdIncome { get; protected set; }
    }
}