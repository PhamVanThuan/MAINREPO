using SAHL.Core.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.ApplicationDomain.Events
{
    public class ApplicantAffordabilitiesAddedEvent : Event
    {
        public ApplicantAffordabilitiesAddedEvent(DateTime date, int clientKey, int applicationNumber, IEnumerable<AffordabilityTypeModel> clientAffordabilityAssessment)
            : base(date)
        {
            this.ClientKey = clientKey;
            this.ApplicationNumber = applicationNumber;
            this.ClientAffordabilityAssessment = clientAffordabilityAssessment;
        }

        public int ClientKey { get; protected set; }

        public int ApplicationNumber { get; protected set; }

        public IEnumerable<AffordabilityTypeModel> ClientAffordabilityAssessment { get; protected set; }

    }
}
