using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class CATSDisbursementAddControls : CATSDisbursementBaseControls
    {
        [FindBy(Id = "ctl00_Main_txtTotalAmount")]
        protected TextField txtTotalAmount { get; set; }

        [FindBy(Id = "ctl00_Main_txtTotalAmount_txtRands")]
        protected TextField txtTotalAmountRands { get; set; }

        [FindBy(Id = "ctl00_Main_txtTotalAmount_txtCents")]
        protected TextField txtTotalAmountCents { get; set; }

        [FindBy(Id = "ctl00_Main_txtDisbursementAmount_txtRands")]
        protected TextField txtTotalAmountToDisburseRands { get; set; }

        [FindBy(Id = "ctl00_Main_txtDisbursementAmount_txtCents")]
        protected TextField txtTotalAmountToDisburseCents { get; set; }

        [FindBy(Id = "ctl00_Main_txtDisbursementReference")]
        protected TextField txtDisbursementReference { get; set; }

        [FindBy(Id = "ctl00_Main_txtDisbursementAmount")]
        protected TextField txtDisbursementAmount { get; set; }

        [FindBy(Id = "ctl00_Main_AddDisbursement")]
        protected Button btnAdd { get; set; }

        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button btnPost { get; set; }

        [FindBy(Id = "ctl00_Main_ddlDisbursementType")]
        protected SelectList DisbursementTypeSelectList { get; set; }

        [FindBy(Id = "ctl00_Main_ddlBankDetails")]
        protected SelectList BankDetailsSelectList { get; set; }

        [FindBy(Id = "ctl00_Main_SaveButton")]
        protected Button btnSave { get; set; }

        [FindBy(Id = "ctl00_Main_DeleteDisbursement")]
        protected Button btnDelete { get; set; }
    }
}