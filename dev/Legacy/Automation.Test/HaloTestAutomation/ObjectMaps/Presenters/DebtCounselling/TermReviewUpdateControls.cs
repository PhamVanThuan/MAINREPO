using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class TermReviewUpdateControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_dteTermReviewDate")]
        public TextField txtReviewDate;

        [FindBy(Id = "ctl00_Main_btnUpdate")]
        public Button btnUpdate;
    }
}