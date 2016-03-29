using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class AttorneyUpdateControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_cmbAttornies")]
        protected SelectList AttorneySelect { get; set; }

        [FindBy(Id = "ctl00_Main_btnUpdate")]
        protected Button btnUpdate { get; set; }
    }
}