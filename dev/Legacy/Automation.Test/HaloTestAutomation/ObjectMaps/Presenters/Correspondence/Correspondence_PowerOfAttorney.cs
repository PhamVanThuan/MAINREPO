using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class Correspondence_PowerOfAttorneyControls : CorrespondenceProcessingControls
    {
        [FindBy(Id = "ctl00_Main_LegalEntitys")]
        protected TextField LegalEntities { get; set; }

        [FindBy(Id = "ctl00_Main_Supervisor")]
        protected TextField Supervisor { get; set; }

        [FindBy(Id = "ctl00_Main_Attorney")]
        protected TextField Attorney { get; set; }

        [FindBy(Id = "ctl00_Main_Gender")]
        protected TextField Gender { get; set; }
    }
}