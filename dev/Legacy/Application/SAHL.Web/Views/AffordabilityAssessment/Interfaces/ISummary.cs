using SAHL.Common.Web.UI;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Web.Views.AffordabilityAssessment.Interfaces
{
    public interface ISummary : IViewBase
    {
        void BindAffordabilityAssessmentsGrid(IEnumerable<AffordabilityAssessmentSummaryModel> affordabilityAssessments);
    }
}