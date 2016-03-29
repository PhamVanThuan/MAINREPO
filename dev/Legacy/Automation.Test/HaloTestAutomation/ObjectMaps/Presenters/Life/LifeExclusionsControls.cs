using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LifeExclusionsControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblClient")]
        protected Span ctl00MainWorkFlowHeader1lblClient { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblClientName")]
        protected Span ctl00MainWorkFlowHeader1lblClientName { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblLoan")]
        protected Span ctl00MainWorkFlowHeader1lblLoan { get; set; }

        [FindBy(Id = "ctl00_Main_WorkFlowHeader1_lblLoanNumber")]
        protected Span ctl00MainWorkFlowHeader1lblLoanNumber { get; set; }

        [FindBy(Id = "ctl00_Main_lblPageHeader")]
        protected Span ctl00MainlblPageHeader { get; set; }

        [FindBy(Id = "ctl00_Main_lblFinal")]
        protected Span ctl00MainlblFinal { get; set; }

        [FindBy(Id = "ctl00_Main_btnNext")]
        protected Button ctl00MainbtnNext { get; set; }

        [FindBy(Id = "ctl00_Main_cbx0")]
        protected CheckBox ctl00Maincbx0 { get; set; }

        [FindBy(Id = "ctl00_Main_DeathOnly")]
        protected RadioButton ctl00_Main_DeathOnly { get; set; }

        [FindBy(Id = "ctl00_Main_DeathAndIPB")]
        protected RadioButton DeathAndIPB { get; set; }
    }
}