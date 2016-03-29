using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class PostTransactionControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_dteEffectiveDate")]
        protected TextField EffectiveDate { get; set; }

        [FindBy(Id = "ctl00_Main_curAmount_txtRands")]
        protected TextField TransactionAmountRands { get; set; }

        [FindBy(Id = "ctl00_Main_curAmount_txtCents")]
        protected TextField TransactionAmountCents { get; set; }

        [FindBy(Id = "ctl00_Main_txtReference")]
        protected TextField Reference { get; set; }

        [FindBy(Id = "ctl00_Main_PostButton")]
        protected Button btnPost { get; set; }

        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_ddlTransactionType")]
        protected SelectList TransactionTypeDropDown { get; set; }

        [FindBy(Id = "ctl00_Main_ddlFinancialService")]
        protected SelectList FinancialService { get; set; }
    }
}