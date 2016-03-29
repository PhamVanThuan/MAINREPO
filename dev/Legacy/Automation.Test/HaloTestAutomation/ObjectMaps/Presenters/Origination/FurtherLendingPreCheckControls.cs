using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class FurtherLendingPreCheckControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnNext")]
        protected Button Next { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button Cancel { get; set; }
    }
}
