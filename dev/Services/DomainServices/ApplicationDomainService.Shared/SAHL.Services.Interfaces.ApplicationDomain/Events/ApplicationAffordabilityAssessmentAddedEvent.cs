using SAHL.Core.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;

namespace SAHL.Services.Interfaces.ApplicationDomain.Events
{
    public class ApplicationAffordabilityAssessmentAddedEvent : Event
    {
        public ApplicationAffordabilityAssessmentAddedEvent(DateTime date, AffordabilityAssessmentModel affordabilityAssessmentModel)
            : base(date)
        {
            this.AffordabilityAssessmentModel = affordabilityAssessmentModel;
        }

        public AffordabilityAssessmentModel AffordabilityAssessmentModel { get; protected set; }
    }
}