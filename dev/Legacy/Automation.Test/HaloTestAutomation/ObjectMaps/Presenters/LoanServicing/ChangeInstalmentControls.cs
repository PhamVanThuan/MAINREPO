using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ChangeInstalmentControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button ChangeInstalment { get; set; }
    }
}