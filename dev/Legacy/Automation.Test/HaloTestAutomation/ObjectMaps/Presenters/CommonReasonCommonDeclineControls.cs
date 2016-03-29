using System.Text.RegularExpressions;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class CommonReasonCommonDeclineControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_cbxReasonType")]
        protected SelectList ReasonType { get; set; }

        [FindBy(Id = "ctl00_Main_lstAvailableReasons")]
        public SelectList ReasonList { get; set; }

        [FindBy(Id = "ctl00_Main_btnAdd")]
        protected Button AddButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnRemove")]
        protected Button RemoveButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button CancelButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnConfirm")]
        protected Button SubmitButton { get; set; }

        [FindBy(Id = "ctl00_Main_lstSelectedReasons")]
        protected SelectList SelectedReasons { get; set; }

        protected TextField CommentBox
        {
            get
            {
                return base.Document.TextField(Find.ById(new Regex("^txtComment[\x20-\x7E]*$")));
            }
        }

        [FindBy(Id = "ctl00_valSummary_Body")]
        protected Div divValidationSummaryBody { get; set; }

        [FindBy(Name = "ctl00$valSummary$ctl05")]
        protected Button divContinueButton { get; set; }
    }
}