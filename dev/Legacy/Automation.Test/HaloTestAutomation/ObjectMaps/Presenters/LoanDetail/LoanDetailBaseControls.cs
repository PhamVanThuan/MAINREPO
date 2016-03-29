using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LoanDetailBaseControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_grdLoanDetail")]
        protected Table LoanDetail { get; set; }

        [FindBy(Id = "ctl00_Main_ddlDetailClass")]
        protected SelectList DetailClass { get; set; }

        [FindBy(Id = "ctl00_Main_ddlDetailType")]
        protected SelectList DetailType { get; set; }

        [FindBy(Id = "ctl00_Main_dateLoanDetailDate")]
        protected TextField DetailDate { get; set; }

        [FindBy(Id = "ctl00_Main_txtDetailAmount")]
        protected TextField Amount { get; set; }

        [FindBy(Id = "ctl00_Main_txtDescription")]
        protected TextField TextDescription { get; set; }

        [FindBy(Id = "ctl00_Main_ddlCancellationType")]
        protected SelectList CancellationType { get; set; }

        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button Cancel { get; set; }
    }
}