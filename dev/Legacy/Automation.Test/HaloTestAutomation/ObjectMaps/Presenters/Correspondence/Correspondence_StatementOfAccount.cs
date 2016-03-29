using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class Correspondence_StatementOfAccountControls : CorrespondenceProcessingControls
    {
        [FindBy(Id = "ctl00_Main_LegalEntitys")]
        public TextField LegalEntities { get; set; }

        [FindBy(Id = "ctl00_Main_SequestrationDate")]
        public TextField SequestrationDate { get; set; }

        [FindBy(Id = "ctl00_Main_Supervisor")]
        public TextField Supervisor { get; set; }

        [FindBy(Id = "ctl00_Main_Format")]
        public SelectList Format { get; set; }
    }
}