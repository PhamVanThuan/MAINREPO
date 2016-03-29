using ObjectMaps.Pages;
using WatiN.Core;

namespace ObjectMaps.Presenters.PersonalLoans
{
    public abstract class PersonalLoanExternalLifePolicySummaryControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_lblInsurer")]
        protected Span Insurer { get; set; }

        [FindBy(Id = "ctl00_Main_lblPolicyNumber")]
        protected Span PolicyNumber { get; set; }

        [FindBy(Id = "ctl00_Main_lblCommencementDate")]
        protected Span CommencementDate { get; set; }

        [FindBy(Id = "ctl00_Main_lblStatus")]
        protected Span Status { get; set; }

        [FindBy(Id = "ctl00_Main_lblClosedate")]
        protected Span CloseDate { get; set; }

        [FindBy(Id = "ctl00_Main_lblSumInsured")]
        protected Span SumInsured { get; set; }

        [FindBy(Id = "ctl00_Main_chkPolicyCeded")]
        protected CheckBox PolicyCeded { get; set; }
    }
}