using SAHL.Core.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.ApplicationDomain.Events
{
    public class ApplicationAffordabilityAssessmentsConfirmedEvent : Event
    {
        public ApplicationAffordabilityAssessmentsConfirmedEvent(DateTime date, IEnumerable<AffordabilityAssessmentModel> affordabilityAssessmentsConfirmed)
            : base(date)
        {
            this.AffordabilityAssessmentsConfirmed = affordabilityAssessmentsConfirmed;
        }

        public IEnumerable<AffordabilityAssessmentModel> AffordabilityAssessmentsConfirmed { get; protected set; }
    }
}