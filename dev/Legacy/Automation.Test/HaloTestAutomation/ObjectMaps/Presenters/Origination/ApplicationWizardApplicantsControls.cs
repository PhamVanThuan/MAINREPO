using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ApplicationWizardApplicantControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlMarketingSource")]
        public SelectList selectMarketingSource { get; set; }

        [FindBy(Id = "ctl00_Main_txtIDNumber")]
        public TextField textfieldIdentityNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtFirstNames")]
        public TextField textfieldFirstNames { get; set; }

        [FindBy(Id = "ctl00_Main_txtSurname")]
        public TextField textfieldSurname { get; set; }

        [FindBy(Id = "ctl00_Main_phContact__CODE")]
        public TextField textfieldContactCode { get; set; }

        [FindBy(Id = "ctl00_Main_phContact__NUMB")]
        public TextField textfieldContactNumber { get; set; }

        [FindBy(Id = "ctl00_Main_tbNumApplicants")]
        public TextField textfieldNumberOfApplicants { get; set; }

        [FindBy(Id = "ctl00_Main_btnNext")]
        public Button btnNext { get; set; }
    }
}