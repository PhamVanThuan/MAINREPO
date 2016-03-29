using System.Collections.Generic;
using System.Text.RegularExpressions;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class CaptureAffordabilityAssessmentControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlLegalEntity")]
        protected SelectList ddlNonApplicantIncomeContributors { get; set; }

        [FindBy(Id = "ctl00_Main_txtNumberOfContributingApplicants")]
        protected TextField txtContributingApplicants { get; set; }

        [FindBy(Id = "ctl00_Main_txtNumberOfHouseholdDependants")]
        protected TextField txtHouseholdDependants { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmit")]
        protected Button Save { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button Cancel { get; set; }

        [FindBy(Id = "ctl00_Main_DeleteAssessmentButton")]
        protected Button DeleteAssessment { get; set; }

        [FindBy(Id = "ctl00_Main_btnAddContributor")]
        protected Button AddContributor { get; set; }

        protected CheckBox chkIncomeAndNonIncomeContributors(string legalEntityKey)
        {
            return base.Document.CheckBox(Find.ByValue(legalEntityKey));
        }
    }
}
