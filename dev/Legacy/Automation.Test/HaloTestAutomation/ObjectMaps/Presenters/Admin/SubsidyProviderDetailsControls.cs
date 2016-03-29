using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class SubsidyProviderDetailsControls : LegalEntityAddressControls
    {
        [FindBy(IdRegex = "SubsidyProvider")]
        public TextField SubsidyProvider { get; set; }

        [FindBy(IdRegex = "ContactPersonEdit")]
        public TextField ContactPersonEdit { get; set; }

        [FindBy(IdRegex = "EmailAddressEdit")]
        public TextField EmailAddressEdit { get; set; }

        [FindBy(Id = "ctl00_Main_txtPhone__CODE")]
        public TextField PhoneCode { get; set; }

        [FindBy(Id = "ctl00_Main_txtPhone__NUMB")]
        public TextField PhoneNumber { get; set; }

        [FindBy(IdRegex = "SubsidyType")]
        public SelectList SubsidyType { get; set; }
    }
}