using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class Orig_QAQueryControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_lblReasonType")]
        protected Span lblReasonType { get; set; }

        [FindBy(Id = "ctl00_Main_SAHLLabel1")]
        protected Span SAHLLabel1 { get; set; }

        [FindBy(Id = "ctl00_Main_lblSelected")]
        protected Span lblSelected { get; set; }

        [FindBy(Id = "ctl00_Main_lblComment")]
        protected Span lblComment { get; set; }

        [FindBy(Id = "ctl00_Main_btnAdd")]
        protected Button btnAdd { get; set; }

        [FindBy(Id = "ctl00_Main_btnRemove")]
        protected Button btnRemove { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_btnConfirm")]
        protected Button btnConfirm { get; set; }

        [FindBy(Id = "ctl00_Main_cbxReasonType")]
        protected SelectList ReasonType { get; set; }

        [FindBy(Id = "ctl00_Main_lstAvailableReasons")]
        protected SelectList AvailableReasons { get; set; }

        [FindBy(Id = "ctl00_Main_lstSelectedReasons")]
        protected SelectList SelectedReasons { get; set; }
    }
}