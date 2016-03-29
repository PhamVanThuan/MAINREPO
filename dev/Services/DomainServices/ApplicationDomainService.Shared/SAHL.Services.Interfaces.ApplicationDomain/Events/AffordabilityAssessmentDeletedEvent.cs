using SAHL.Core.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;

namespace SAHL.Services.Interfaces.ApplicationDomain.Commands
{
    public class AffordabilityAssessmentDeletedEvent : Event
    {
        public AffordabilityAssessmentDeletedEvent(DateTime date, AffordabilityAssessmentModel affordabilityAssessmentModel)
            : base(date)
        {
            this.AffordabilityAssessmentModel = affordabilityAssessmentModel;
        }

        public AffordabilityAssessmentModel AffordabilityAssessmentModel { get; protected set; }
    }
}