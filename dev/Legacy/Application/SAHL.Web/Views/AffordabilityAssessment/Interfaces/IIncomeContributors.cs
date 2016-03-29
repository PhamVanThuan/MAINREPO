using SAHL.Common.Web.UI;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Web.Views.AffordabilityAssessment.Interfaces
{
    public interface IIncomeContributors : IViewBase
    {
        void BindApplicantContributors(IEnumerable<LegalEntityModel> applicantContributors, IEnumerable<int> selectedLegalEntities);

        void BindNonApplicantContributors(IEnumerable<LegalEntityModel> nonApplicantContributors, IEnumerable<int> selectedLegalEntities);

        void BindLegalEntityDropdownList(IEnumerable<LegalEntityModel> applicantContributors);

        event EventHandler OnAddContributorButtonClicked;

        event EventHandler OnSubmitButtonClicked;

        event EventHandler OnCancelButtonClicked;

        int SelectedLegalEntityKey { get; set; }

        int NumberOfContributingApplicants { get; set; }

        int NumberOfHouseholdDependants { get; set; }

        IEnumerable<int> GetContributorsList { get; }
    }
}