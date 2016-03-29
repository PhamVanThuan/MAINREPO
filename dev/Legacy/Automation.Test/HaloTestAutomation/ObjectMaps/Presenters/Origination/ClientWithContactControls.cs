using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ClientWithContactControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_dteContactDate")]
        public TextField ContactDate { get; set; }

        [FindBy(Id = "ctl00_Main_txtComments")]
        public TextField Comments { get; set; }
    }
}