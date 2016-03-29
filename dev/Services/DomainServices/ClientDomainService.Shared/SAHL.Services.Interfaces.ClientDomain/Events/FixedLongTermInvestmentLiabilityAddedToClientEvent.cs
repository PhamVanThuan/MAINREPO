using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Events
{
    public class FixedLongTermInvestmentLiabilityAddedToClientEvent : Event
    {
        public FixedLongTermInvestmentLiabilityAddedToClientEvent(DateTime date, string companyName, double liabilityValue)
            : base(date)
        {
            this.CompanyName = companyName;
            this.LiabilityValue = liabilityValue;
        }

        public string CompanyName { get; protected set; }

        public double LiabilityValue { get; protected set; }
    }
}