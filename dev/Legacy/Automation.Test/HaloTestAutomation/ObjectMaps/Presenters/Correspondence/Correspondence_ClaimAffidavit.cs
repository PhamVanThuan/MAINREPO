using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class Correspondence_ClaimAffidavitControls : CorrespondenceProcessingControls
    {
        [FindBy(Id = "ctl00_Main_LegalEntitys")]
        protected TextField LegalEntities { get; set; }

        [FindBy(Id = "ctl00_Main_LegalEntityIDs")]
        protected TextField LegalEntityIDs { get; set; }

        [FindBy(Id = "ctl00_Main_Supervisor")]
        protected TextField Supervisor { get; set; }

        [FindBy(Id = "ctl00_Main_SequestrationType")]
        protected SelectList SequestrationType { get; set; }

        [FindBy(Id = "ctl00_Main_SequestrationDate")]
        protected TextField SequestrationDate { get; set; }

        [FindBy(Id = "ctl00_Main_Format")]
        protected SelectList Format { get; set; }
    }
}