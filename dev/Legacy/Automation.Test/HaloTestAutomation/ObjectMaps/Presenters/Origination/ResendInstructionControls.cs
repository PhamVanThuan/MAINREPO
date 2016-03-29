using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ResendInstructionControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlDeedsOffice")]
        protected SelectList DeedsOfficeDropDown { get; set; }

        [FindBy(Id = "ctl00_Main_ddlRegistrationAttorney")]
        protected SelectList AttorneyDropDown { get; set; }

        [FindBy(Id = "ctl00_Main_btnUpdateButton")]
        protected Button UpdateButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnSendInstruction")]
        protected Button SendInstructionButton { get; set; }
    }
}