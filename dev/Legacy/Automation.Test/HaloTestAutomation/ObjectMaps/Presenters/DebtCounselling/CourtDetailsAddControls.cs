using System.Text.RegularExpressions;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class CourtDetailsAddControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnSubmit")]
        protected Button btnAdd { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_ddlHearingType")]
        protected SelectList HearingType { get; set; }

        [FindBy(Id = "ctl00_Main_ddlAppearanceType")]
        protected SelectList AppearanceType { get; set; }

        [FindBy(Id = "ctl00_Main_txtCourtSearch")]
        protected TextField Court { get; set; }

        [FindBy(Id = "ctl00_Main_txtCaseNumber")]
        protected TextField CaseNumber { get; set; }

        [FindBy(Id = "ctl00_Main_dteHearingDate")]
        protected TextField HearingDate { get; set; }

        protected Div SAHLAutoComplete_DefaultItem(string Court)
        {
            return base.Document.Div("SAHLAutoCompleteDiv").Div(Find.ByText(new Regex("^" + Court + "[\x20-\x7E]*$")));
        }

        [FindBy(Id = "SAHLAutoCompleteDiv_iframe")]
        protected Element SAHLAutoCompleteDiv_iframe { get; set; }

        [FindBy(Id = "ctl00_Main_txtComments")]
        protected TextField Comments { get; set; }

        [FindBy(Id = "ctl00_Main_lblHearingDate")]
        protected Span Date { get; set; }
    }
}