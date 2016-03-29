using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LifeDeclarationControls : BasePageControls
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

        [FindBy(Id = "ctl00_Main_btnNext")]
        protected Button ctl00MainbtnNext { get; set; }

        [FindBy(Id = "ctl00_Main_cbx0")]
        protected CheckBox ctl00Maincbx0 { get; set; }

        [FindBy(Id = "ctl00_Main_cbx1")]
        protected CheckBox ctl00Maincbx1 { get; set; }

        [FindBy(Id = "ctl00_Main_cbx2")]
        protected CheckBox ctl00Maincbx2 { get; set; }

        [FindBy(Id = "ctl00_Main_cbx3")]
        protected CheckBox ctl00Maincbx3 { get; set; }

        [FindBy(Id = "ctl00_Main_cbx4")]
        protected CheckBox ctl00Maincbx4 { get; set; }

        [FindBy(Id = "ctl00_Main_cbx5")]
        protected CheckBox ctl00Maincbx5 { get; set; }
    }
}