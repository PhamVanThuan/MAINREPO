using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class HOCFSSummaryUpdateControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlStatus")]
        protected SelectList HOCStatus { get; set; }

        [FindBy(Id = "ctl00_Main_ddlHOCInsurer")]
        protected SelectList HOCInsurer { get; set; }

        [FindBy(Id = "ctl00_Main_ddlSubsidenceDescription")]
        protected SelectList SubsidenceDescription { get; set; }

        [FindBy(Id = "ctl00_Main_ddlConstructionDescription")]
        protected SelectList ConstructionDescription { get; set; }

        [FindBy(Id = "ctl00_Main_txtTotalHOCSumInsured")]
        protected TextField HOCSumInsured { get; set; }

        [FindBy(Id = "ctl00_Main_txtHOCPolicyNumber")]
        protected TextField HOCPolicyNumber { get; set; }

        [FindBy(Id = "ctl00_Main_chkCeded")]
        protected CheckBox Ceded { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmitButton")]
        protected Button btnUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancelButton")]
        protected Button btnCancel { get; set; }
    }
}