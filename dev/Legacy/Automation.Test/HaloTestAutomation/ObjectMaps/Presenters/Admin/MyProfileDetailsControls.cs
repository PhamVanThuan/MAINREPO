using ObjectMaps.Pages;
using WatiN.Core;

namespace ObjectMaps.Presenters.Admin
{
    public abstract class MyProfileDetailsControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_lblFirstNames")]
        protected Span FirstName { get; set; }

        [FindBy(Id = "ctl00_Main_lblSurname")]
        protected Span Surname { get; set; }

        [FindBy(Id = "ctl00_Main_lblEmailAddress")]
        protected Span EmailAddress { get; set; }

        [FindBy(Id = "ctl00_Main_lblSalutation")]
        protected Span Salutation { get; set; }

        [FindBy(Id = "ctl00_Main_lblInitials")]
        protected Span Initials { get; set; }

        [FindBy(Id = "ctl00_Main_lblPreferredName")]
        protected Span PreferredName { get; set; }

        [FindBy(Id = "ctl00_Main_lblEducation")]
        protected Span Education { get; set; }

        [FindBy(Id = "ctl00_Main_lblHomeLanguage")]
        protected Span HomeLanguage { get; set; }

        [FindBy(Id = "ctl00_Main_lblHomePhone")]
        protected Span HomePhone { get; set; }

        [FindBy(Id = "ctl00_Main_lblWorkPhone")]
        protected Span WorkPhone { get; set; }

        [FindBy(Id = "ctl00_Main_lblFaxNumber")]
        protected Span FaxNumber { get; set; }

        [FindBy(Id = "ctl00_Main_lblCellphoneNumber")]
        protected Span CellphoneNumber { get; set; }
    }
}