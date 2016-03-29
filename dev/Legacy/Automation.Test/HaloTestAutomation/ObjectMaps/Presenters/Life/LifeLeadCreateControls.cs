using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LifeLeadCreateControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtAccountNumber")]
        protected TextField ctl00MaintxtAccountNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtSurname")]
        protected TextField ctl00MaintxtSurname { get; set; }

        [FindBy(Id = "ctl00_Main_txtFirstnames")]
        protected TextField ctl00MaintxtFirstnames { get; set; }

        [FindBy(Id = "ctl00_Main_lblAccountNumber")]
        protected Span ctl00MainlblAccountNumber { get; set; }

        [FindBy(Id = "ctl00_Main_lblSurname")]
        protected Span ctl00MainlblSurname { get; set; }

        [FindBy(Id = "ctl00_Main_lblFirstNames")]
        protected Span ctl00MainlblFirstNames { get; set; }

        [FindBy(Id = "ctl00_Main_lblConsultant")]
        protected Span ctl00MainlblConsultant { get; set; }

        [FindBy(Id = "ctl00_Main_btnSearch")]
        protected Button ctl00MainbtnSearch { get; set; }

        [FindBy(Id = "ctl00_Main_btnCreateLeads")]
        protected Button ctl00MainbtnCreateLeads { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button ctl00MainbtnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_selMaxResults")]
        protected SelectList ctl00MainselMaxResults { get; set; }

        [FindBy(Id = "ctl00_Main_ddlConsultant")]
        protected SelectList ctl00MainddlConsultant { get; set; }

        [FindBy(Id = "ctl00_Main_SearchGrid_ctl01_ctl00")]
        protected CheckBox ctl00MainSearchGridctl01ctl00 { get; set; }
    }
}