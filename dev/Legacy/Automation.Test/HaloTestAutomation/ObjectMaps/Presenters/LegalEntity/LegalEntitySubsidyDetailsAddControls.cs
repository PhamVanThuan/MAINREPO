using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LegalEntitySubsidyDetailsAddControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlAccountKey")]
        protected SelectList ddlAccountKey { get; set; }

        [FindBy(Id = "ctl00_Main_txtSubsidyProvider")]
        protected TextField txtSubsidyProvider { get; set; }

        [FindBy(Id = "ctl00_Main_txtSalaryNumber")]
        protected TextField txtSalaryNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtPaypoint")]
        protected TextField txtPaypoint { get; set; }

        [FindBy(Id = "ctl00_Main_txtRank")]
        protected TextField txtRank { get; set; }

        [FindBy(Id = "ctl00_Main_txtNotch")]
        protected TextField txtNotch { get; set; }

        [FindBy(Id = "ctl00_Main_currStopOrder_txtRands")]
        protected TextField currStopOrder_txtRands { get; set; }

        [FindBy(Id = "ctl00_Main_currStopOrder_txtCents")]
        protected TextField currStopOrder_txtCents { get; set; }

        [FindBy(Id = "ctl00_Main_btnBack")]
        protected Button btnBack { get; set; }

        [FindBy(Id = "ctl00_Main_btnSave")]
        protected Button btnSave { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "SAHLAutoCompleteDiv")]
        protected Div divSAHLAutoCompleteDiv { get; set; }

        protected DivCollection divsSAHLAutoCompleteItems
        {
            get
            {
                return divSAHLAutoCompleteDiv.Divs;
            }
        }

        protected Div divSAHLAutoCompleteItem(string InnerText)
        {
            return divsSAHLAutoCompleteItems.Filter(Find.ByText(InnerText))[0];
        }
    }
}