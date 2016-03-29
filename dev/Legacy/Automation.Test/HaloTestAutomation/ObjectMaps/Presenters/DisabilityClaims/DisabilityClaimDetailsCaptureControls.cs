using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class DisabilityClaimDetailsCaptureControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_dtDateOfDiagnosis")]
        protected TextField DateOfDiagnosis { get; set; }

        [FindBy(Id = "ctl00_Main_ddlDisabilityType")]
        protected SelectList DisabilityType { get; set; }

        [FindBy(Id = "ctl00_Main_txtOtherDisabilityComments")]
        protected TextField OtherDisabilityComments { get; set; }

        [FindBy(Id = "ctl00_Main_txtClaimantOccupation")]
        protected TextField ClaimantOccupation { get; set; }

        [FindBy(Id = "ctl00_Main_dtLastDateWorked")]
        protected TextField LastDateWorked { get; set; }

        [FindBy(Id = "ctl00_Main_dtExpectedReturnToWorkDate")]
        protected TextField ExpectedReturnToWorkDate { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmit")]
        protected Button Submit { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button Cancel { get; set; }
    }
}