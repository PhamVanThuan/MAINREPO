using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LifeClientSuperSearchAddWorkFlowControls : BasePageControls
    {
        [FindBy(Text = "Basic Search")]
        protected Link BasicSearch { get; set; }

        [FindBy(Text = "Advanced Search")]
        protected Link AdvancedSearch { get; set; }

        [FindBy(Id = "ctl00_Main_txtFirstName")]
        protected TextField ctl00MaintxtFirstName { get; set; }

        [FindBy(Id = "ctl00_Main_txtSurname")]
        protected TextField ctl00MaintxtSurname { get; set; }

        [FindBy(Id = "ctl00_Main_txtID")]
        protected TextField ctl00MaintxtID { get; set; }

        [FindBy(Id = "ctl00_Main_txtSalaryNumber")]
        protected TextField ctl00MaintxtSalaryNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtAccountKey")]
        protected TextField ctl00MaintxtAccountKey { get; set; }

        [FindBy(Id = "ctl00_Main_txtSearch")]
        protected TextField ctl00MaintxtSearch { get; set; }

        [FindBy(Id = "ctl00_Main_btnSearchBasic")]
        protected Button ctl00MainbtnSearchBasic { get; set; }

        [FindBy(Id = "ctl00_Main_btnSearchAdvanced")]
        protected Button ctl00MainbtnSearchAdvanced { get; set; }

        [FindBy(Id = "ctl00_Main_btnNewLegalEntity")]
        protected Button ctl00MainbtnNewLegalEntity { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button ctl00MainbtnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_ddlLeType")]
        protected SelectList ctl00MainddlLeType { get; set; }

        [FindBy(Id = "ctl00_Main_cbxAccountType")]
        protected SelectList ctl00MaincbxAccountType { get; set; }

        [FindBy(Id = "tblBasicOptions")]
        protected Table tblBasicOptions { get; set; }

        [FindBy(Id = "tblAdvancedOptions")]
        protected Table tblAdvancedOptions { get; set; }

        [FindBy(Id = "ctl00_Main_gridSearchResults")]
        protected Table ctl00MaingridSearchResults { get; set; }
    }
}