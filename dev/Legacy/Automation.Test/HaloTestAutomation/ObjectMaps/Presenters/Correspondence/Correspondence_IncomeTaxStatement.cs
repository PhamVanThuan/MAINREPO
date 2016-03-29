using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class Correspondence_IncomeTaxStatementControls : CorrespondenceProcessingControls
    {
        [FindBy(Id = "ctl00_Main_Year")]
        public SelectList TaxPeriod { get; set; }

        [FindBy(Id = "ctl00_Main_Format")]
        public SelectList Format { get; set; }
    }
}