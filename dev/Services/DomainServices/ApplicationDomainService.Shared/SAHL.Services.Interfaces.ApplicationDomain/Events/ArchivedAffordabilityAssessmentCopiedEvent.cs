using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.ApplicationDomain.Events
{
    public class ArchivedAffordabilityAssessmentCopiedEvent : Event
    {
        public ArchivedAffordabilityAssessmentCopiedEvent(DateTime date, 
                                                          AffordabilityAssessmentDataModel archivedAffordabilityAssessment, 
                                                          AffordabilityAssessmentDataModel newAffordabilityAssessment, 
                                                          IEnumerable<AffordabilityAssessmentItemDataModel> 
                                                          newAffordabilityAssessmentItems)
            : base(date)
        {
            this.ArchivedAffordabilityAssessment = archivedAffordabilityAssessment;
            this.NewAffordabilityAssessment = newAffordabilityAssessment;
            this.NewAffordabilityAssessmentItems = newAffordabilityAssessmentItems;
        }

        public AffordabilityAssessmentDataModel ArchivedAffordabilityAssessment { get; protected set; }

        public AffordabilityAssessmentDataModel NewAffordabilityAssessment { get; protected set; }

        public IEnumerable<AffordabilityAssessmentItemDataModel> NewAffordabilityAssessmentItems { get; protected set; }
    }
}