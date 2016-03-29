using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class CAPCreateSearchControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtAccountNumber")]
        protected TextField txtAccountNumber;

        [FindBy(Id = "ctl00_Main_SearchButton")]
        protected Button SearchButton;

        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button CancelButton;

        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button SubmitButton;

        [FindBy(Id = "ctl00_Main_ddlAccountType")]
        protected SelectList ddlAccountType;

        [FindBy(Id = "ctl00_valSummary_Body")]
        public Div divValidationSummaryBody;

        public ElementCollection listErrorMessages
        {
            get
            {
                Div validationSummaryBody = divValidationSummaryBody;
                return validationSummaryBody.ElementsWithTag("LI");
            }
        }
    }
}