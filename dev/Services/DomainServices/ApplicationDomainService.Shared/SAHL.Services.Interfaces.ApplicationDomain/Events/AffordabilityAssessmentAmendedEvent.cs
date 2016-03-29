using SAHL.Core.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;

namespace SAHL.Services.Interfaces.ApplicationDomain.Events
{
    public class AffordabilityAssessmentAmendedEvent : Event
    {
        public AffordabilityAssessmentAmendedEvent(DateTime date, AffordabilityAssessmentModel affordabilityAssessmentModel)
            : base(date)
        {
            this.AffordabilityAssessmentModel = affordabilityAssessmentModel;
        }

        public AffordabilityAssessmentModel AffordabilityAssessmentModel { get; protected set; }
    }
}