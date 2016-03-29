using SAHL.Common.Web.UI;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Web.Views.AffordabilityAssessment.Interfaces
{
    public interface IHistory : IViewBase
    {
        int SelectedAffordabilityAssessmentKey { get; }

        bool ShowApplicationKeyInGrid { get; set;  }

        bool ShowGeneralStatusInGrid { get; set; }

        string AffordabilityAssessmentGridHeaderCaption { get; set; }

        event EventHandler OnViewAssessmentButtonClicked;

        void BindAffordabilityAssessmentsGrid(IEnumerable<AffordabilityAssessmentSummaryModel> affordabilityAssessments);
    }
}