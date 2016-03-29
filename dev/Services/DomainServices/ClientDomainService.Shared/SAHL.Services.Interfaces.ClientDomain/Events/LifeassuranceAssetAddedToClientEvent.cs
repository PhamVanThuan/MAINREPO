using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Events
{
    public class LifeAssuranceAssetAddedToClientEvent : Event
    {
        public LifeAssuranceAssetAddedToClientEvent(DateTime date, string companyName, double surrenderValue)
            : base(date)
        {
            this.CompanyName = companyName;
            this.SurrenderValue = surrenderValue;
        }

        public string CompanyName { get; protected set; }

        public double SurrenderValue { get; protected set; }
    }
}
