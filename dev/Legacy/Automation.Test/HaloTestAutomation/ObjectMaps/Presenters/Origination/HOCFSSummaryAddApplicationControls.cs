using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class HOCFSSummaryAddApplicationControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtTotalHOCSumInsured")]
        protected TextField ctl00MaintxtTotalHOCSumInsured { get; set; }

        [FindBy(Id = "ctl00_Main_txtHOCPolicyNumber")]
        protected TextField ctl00MaintxtHOCPolicyNumber { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancelButton")]
        protected Button ctl00MainbtnCancelButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmitButton")]
        protected Button ctl00MainbtnSubmitButton { get; set; }

        [FindBy(Id = "ctl00_Main_ddlHOCInsurer")]
        protected SelectList ctl00MainddlHOCInsurer { get; set; }

        [FindBy(Id = "ctl00_Main_chkCeded")]
        protected CheckBox ctl00MainchkCeded { get; set; }
    }
}