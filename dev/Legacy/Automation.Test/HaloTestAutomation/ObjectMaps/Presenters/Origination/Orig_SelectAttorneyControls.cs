using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class Orig_SelectAttorneyControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtPreferredAttorney")]
        protected TextField txtPreferredAttorney { get; set; }

        [FindBy(Id = "ctl00_Main_lblheader")]
        protected Span lblheader { get; set; }

        [FindBy(Id = "ctl00_Main_btnUpdateButton")]
        protected Button btnUpdateButton { get; set; }

        [FindBy(Id = "ctl00_Main_ddlDeedsOffice")]
        protected SelectList ddlDeedsOffice { get; set; }

        [FindBy(Id = "ctl00_Main_ddlRegistrationAttorney")]
        protected SelectList ddlRegistrationAttorney { get; set; }
    }
}