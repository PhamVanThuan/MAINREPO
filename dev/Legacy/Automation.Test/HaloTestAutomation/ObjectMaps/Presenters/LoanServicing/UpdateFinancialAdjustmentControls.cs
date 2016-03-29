using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class UpdateFinancialAdjustmentControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_gvUpdateFinancialAdjustments")]
        protected Table FinancialAdjustmentTable { get; set; }

        [FindBy(Id = "ctl00_Main_gvUpdateFinancialAdjustments_DXEditor5_I")]
        protected TextField FinancialAdjustmentStatusDropDownList { get; set; }

        [FindBy(Id = "ctl00_Main_gvUpdateFinancialAdjustments_DXEditor5_DDD_L_LBI0T0")]
        protected TableCell ActiveFinancialAdjustmentStatusCell { get; set; }

        [FindBy(Id = "ctl00_Main_gvUpdateFinancialAdjustments_DXEditor5_DDD_L_LBI1T0")]
        protected TableCell CancelledFinancialAdjustmentStatusCell { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button CancelButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnUpdate")]
        protected Button UpdateButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnUpdate")]
        protected TableCell CanceledFinancialAdjustmentStatusCell { get; set; }
    }
}