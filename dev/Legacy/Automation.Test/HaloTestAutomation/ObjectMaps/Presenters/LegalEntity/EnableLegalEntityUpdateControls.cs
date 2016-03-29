using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class EnableLegalEntityUpdateControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_labelMessage")]
        protected Span ctl00MainlabelMessage { get; set; }

        [FindBy(Id = "ctl00_Main_labelQuestion")]
        protected Span ctl00MainlabelQuestion { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmitButton")]
        protected Button btnYes { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancelButton")]
        protected Button btnNo { get; set; }
    }
}