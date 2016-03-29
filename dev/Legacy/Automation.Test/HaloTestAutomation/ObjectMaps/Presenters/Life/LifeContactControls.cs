using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LifeContactControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnUpdateDetails")]
        protected Button ctl00MainbtnUpdateDetails { get; set; }

        [FindBy(Id = "ctl00_Main_btnAddAddress")]
        protected Button ctl00MainbtnAddAddress { get; set; }

        [FindBy(Id = "ctl00_Main_btnUpdateAddress")]
        protected Button ctl00MainbtnUpdateAddress { get; set; }

        [FindBy(Id = "ctl00_Main_btnNext")]
        protected Button ctl00MainbtnNext { get; set; }

        [FindBy(Id = "tAssuredLivesGrid")]
        protected Table tAssuredLivesGrid { get; set; }

        [FindBy(Id = "tAssuredLivesDetail")]
        protected Table tAssuredLivesDetail { get; set; }

        [FindBy(Id = "ctl00_Main_LegalEntityGrid")]
        protected Table ctl00MainLegalEntityGrid { get; set; }

        [FindBy(Id = "ctl00_Main_AddressGrid")]
        protected Table ctl00MainAddressGrid { get; set; }
    }
}