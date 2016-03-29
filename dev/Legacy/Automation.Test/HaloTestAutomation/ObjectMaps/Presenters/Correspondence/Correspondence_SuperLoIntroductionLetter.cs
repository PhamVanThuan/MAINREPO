using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class Correspondence_SuperLoIntroductionLetterControls : CorrespondenceProcessingControls
    {
        [FindBy(Id = "ctl00_Main_ADUserKey")]
        protected SelectList HelpDeskUser { get; set; }
    }
}