using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ITCApplicationControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnDoEnquiry")]
        protected Button doEnquiry { get; set; }
    }
}