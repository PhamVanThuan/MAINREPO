using WatiN.Core;

namespace ObjectMaps.Presenters.PersonalLoans
{
    public abstract class PersonalLoanAlteredApprovalControls : PersonalLoanCalculatorControls
    {
        [FindBy(Id = "ctl00_Main_btnCreateApplication")]
        protected Button UpdateApplication { get; set; }
    }
}