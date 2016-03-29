using SAHL.Common.Web.UI;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Web.Views.AffordabilityAssessment.Interfaces
{
    public interface IDelete : IViewBase
    {
        int SelectedUnconfirmedAffordabilityAssessmentKey { get; }

        event EventHandler OnDeleteAssessmentButtonClicked;

        void BindUnconfirmedGrid(IEnumerable<AffordabilityAssessmentSummaryModel> unconfirmedAffordabilityAssessments);
    }
}