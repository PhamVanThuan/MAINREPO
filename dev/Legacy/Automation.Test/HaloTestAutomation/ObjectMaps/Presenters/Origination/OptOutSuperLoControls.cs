using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class OptOutSuperLoControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_SuperLoOptOut")]
        protected Button btnOptOutSuperLo { get; set; }
    }
}