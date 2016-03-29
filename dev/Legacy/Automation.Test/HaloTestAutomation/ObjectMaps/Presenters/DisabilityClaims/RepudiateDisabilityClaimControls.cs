using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class RepudiateDisabilityClaimControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_lstAvailableReasons")]
        protected SelectList AvailableReasons { get; set; }

        [FindBy(Id = "ctl00_Main_lstSelectedReasons")]
        protected SelectList SelectedReasons { get; set; }

        [FindBy(Id = "ctl00_Main_btnConfirm")]
        protected Button Submit { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button Cancel { get; set; }

        [FindBy(Id = "ctl00_Main_btnAdd")]
        protected Button AddReason { get; set; }

        [FindBy(Id = "ctl00_Main_btnRemove")]
        protected Button RemoveReason { get; set; }
    }
}