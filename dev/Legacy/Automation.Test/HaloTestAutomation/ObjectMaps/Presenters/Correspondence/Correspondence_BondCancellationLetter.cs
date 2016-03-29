using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class Correspondence_BondCancellationLetterControls : CorrespondenceProcessingControls
    {
        [FindBy(Id = "ctl00_Main_CancellationType")]
        public SelectList CancellationType { get; set; }
    }
}