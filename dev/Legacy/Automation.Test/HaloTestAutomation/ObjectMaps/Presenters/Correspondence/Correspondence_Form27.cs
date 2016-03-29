using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class Correspondence_Form27Controls : CorrespondenceProcessingControls
    {
        [FindBy(Id = "ctl00_Main_UserName")]
        protected TextField UserName { get; set; }

        [FindBy(Id = "ctl00_Main_Format")]
        protected SelectList Formats { get; set; }
    }
}