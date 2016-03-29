using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class Correspondence_CapLetterControls : CorrespondenceProcessingControls
    {
        [FindBy(Id = "ctl00_Main_AccountKey")]
        protected TextField AccountNumber { get; set; }

        [FindBy(Id = "ctl00_Main_Format")]
        protected SelectList Formats { get; set; }
    }
}