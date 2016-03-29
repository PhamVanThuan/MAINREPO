using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ClientSuperSearchControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtFirstName")]
        protected TextField txtFirstName { get; set; }

        [FindBy(Id = "ctl00_Main_txtSurname")]
        protected TextField txtSurname { get; set; }

        [FindBy(Id = "ctl00_Main_txtID")]
        protected TextField txtID { get; set; }

        [FindBy(Id = "ctl00_Main_txtSalaryNumber")]
        protected TextField txtSalaryNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtAccountKey")]
        protected TextField txtAccountKey { get; set; }

        [FindBy(Id = "ctl00_Main_txtSearch")]
        protected TextField txtSearch { get; set; }

        [FindBy(Id = "ctl00_Main_ddlLeType")]
        protected SelectList LegalEntityType { get; set; }

        [FindBy(Id = "ctl00_Main_cbxAccountType")]
        protected SelectList AccountType { get; set; }

        [FindBy(Id = "ctl00_Main_btnSearchBasic")]
        protected Button btnSearch { get; set; }

        protected Link LegalEntitySelect(string LegalEntityName)
        {
            return base.Document.Link(Find.ByTitle(LegalEntityName));
        }

        protected Link LegalEntitySelectbyHref(string LegalEntityKey)
        {
            return base.Document.Link(Find.By("href", "javascript:__doPostBack('ctl00$Main$gridSearchResults','" + LegalEntityKey + "')"));
        }

        [FindBy(Id = "ctl00_Main_gridSearchResults")]
        protected Table SearchResultsTable { get; set; }

        [FindBy(Value = "New Legal Entity")]
        protected Button NewLegalEntity { get; set; }

        [FindBy(Text = "Advanced Search")]
        protected Link AdvancedSearchLink { get; set; }

        [FindBy(Text = "Basic Search")]
        protected Link BasicSearchLink { get; set; }

        [FindBy(Id = "ctl00_Main_btnSearchAdvanced")]
        protected Button btnAdvancedSearch { get; set; }

        [FindBy(Id = "ctl00_Main_btnNewLegalEntity")]
        protected Button btnNewLegalEntity { get; set; }

        [FindBy(Id = "ctl00_Main_btnNewLegalEntity")]
        protected Button NewAssuredLife { get; set; }
    }
}