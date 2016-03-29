using ObjectMaps.Pages;
using WatiN.Core;

namespace ObjectMaps.Presenters.PersonalLoans
{
    public abstract class PersonalLoanChangeInstalmentControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnCalculate")]
        protected Button btnCalculate { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmit")]
        protected Button btnChangeInstalment { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_lblNewInstalment")]
        protected Span lblNewLoanInstalment { get; set; }

        [FindBy(Id = "ctl00_Main_lblMonthlyServiceFee")]
        protected Span lblMonthlyServiceFee { get; set; }

        [FindBy(Id = "ctl00_Main_lblTotalInstalment")]
        protected Span lblNewTotalInstalment { get; set; }

        [FindBy(Id = "ctl00_Main_txtComments")]
        protected TextField txtComments { get; set; }

        [FindBy(Id = "ctl00_Main_lblCreditLifePremium")]
        protected Span lblCreditLifePremium { get; set; }
    }
}