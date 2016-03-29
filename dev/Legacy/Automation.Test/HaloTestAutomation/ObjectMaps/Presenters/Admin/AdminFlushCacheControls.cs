using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class AdminFlushCacheControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtUserAccess")]
        protected TextField txtUserLogin { get; set; }

        [FindBy(Id = "ctl00_Main_txtUserAccess")]
        protected SelectList LookupDropdown { get; set; }

        [FindBy(Id = "ctl00_Main_btnUserAccess")]
        protected Button btnClearUserAccess { get; set; }

        [FindBy(Id = "ctl00_Main_btnLookupAll")]
        protected Button btnClearAllLookups { get; set; }

        [FindBy(Id = "ctl00_Main_btnLookup")]
        protected Button btnClearLookup { get; set; }

        [FindBy(Id = "ctl00_Main_btnUIStatement")]
        protected Button btnClearUIStatements { get; set; }

        [FindBy(Id = "ctl00_Main_btnOrgStructure")]
        protected Button btnClearX2Cache { get; set; }
    }
}