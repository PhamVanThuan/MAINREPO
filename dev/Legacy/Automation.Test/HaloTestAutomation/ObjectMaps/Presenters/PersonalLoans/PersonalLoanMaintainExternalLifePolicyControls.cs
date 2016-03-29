using ObjectMaps.Pages;
using WatiN.Core;

namespace ObjectMaps.Presenters.PersonalLoans
{
    public abstract class PersonalLoanMaintainExternalLifePolicyControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlInsurer")]
        protected SelectList Insurer { get; set; }

        [FindBy(Id = "ctl00_Main_txtPolicyNumber")]
        protected TextField PolicyNumber { get; set; }

        [FindBy(Id = "ctl00_Main_dtCommencementDate")]
        protected TextField CommencementDate { get; set; }

        [FindBy(Id = "ctl00_Main_ctl00_Main_dtCommencementDateImage")]
        protected Element CommencementDatePicker { get; set; }

        [FindBy(Id = "ctl00_Main_ddlStatus")]
        protected SelectList Status { get; set; }

        [FindBy(Id = "ctl00_Main_dtClosedate")]
        protected TextField CloseDate { get; set; }

        [FindBy(Id = "ctl00_Main_ctl00_Main_dtClosedateImage")]
        protected Element CloseDatePicker { get; set; }

        [FindBy(Id = "ctl00_Main_txtSumInsured")]
        protected TextField SumInsured { get; set; }

        [FindBy(Id = "ctl00_Main_chkPolicyCeded")]
        protected CheckBox PolicyCeded { get; set; }

        [FindBy(Id = "ctl00_Main_btnSave")]
        protected Button Save { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button Cancel { get; set; }
    }
}